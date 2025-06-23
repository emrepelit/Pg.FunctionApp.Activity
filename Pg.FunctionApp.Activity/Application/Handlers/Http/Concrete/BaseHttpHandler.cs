using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Pg.FunctionApp.Activity.Core.Exceptions;
using Polly;

namespace Pg.FunctionApp.Activity.Application.Handlers.Http.Concrete;

public abstract class BaseHttpHandler<T> where T : class
{
    private const int TotalRetry = 5;
    private readonly TimeSpan _retryInterval = TimeSpan.FromSeconds(3);
    
    private readonly HttpClient _httpClient;
    private readonly ILogger<T> _logger;

    protected BaseHttpHandler(HttpClient httpClient, ILogger<T> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected async Task<HttpResponseMessage> SendPostRequestAsync(string endpoint, object payload)
    {
        var jsonPayload = JsonSerializer.Serialize(payload);
        var contentToSend = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // I checked the following link to remind myself the  usage of it: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
        return await _httpClient.PostAsync(endpoint, contentToSend);
    }

    protected async Task<TResponse?> SendGetRequestAsync<TResponse>(
        string endpoint)
    {
        /* I checked the following link to remind myself the usage of polly:
        https://www.codeproject.com/Articles/5378791/How-to-Use-Polly-In-Csharp-Easily-Handle-Faults-An */
        var policy = Policy
            .HandleResult<HttpResponseMessage>(r =>
                r.StatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable)
            .WaitAndRetryAsync(TotalRetry, _ => _retryInterval,
                (response, _, tryCount, _) =>
                {
                    _logger.LogWarning(
                        $"Status code= {response.Result?.StatusCode}... Retrying in {tryCount} of {TotalRetry} attempts.");
                });
        // I feel like it makes sense to increase retry interval with some sort of calculation based on re-try attempts above, but I'm keeping it simple.

        var response = await policy.ExecuteAsync(() => _httpClient.GetAsync(endpoint));

        if (response.StatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable)
        {
            throw new WorkflowServiceUnavailableException(
                $"{endpoint} unavailable. Re-tried {TotalRetry} times. Status= {response.StatusCode}, Reason= {response.ReasonPhrase}",
                response.StatusCode,
                TotalRetry);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new WorkflowNotFoundException(
                $"Workflow not found. Status= {response.StatusCode}, Reason= {response.ReasonPhrase}",
                response.StatusCode);
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<TResponse>(jsonString);
    }
}