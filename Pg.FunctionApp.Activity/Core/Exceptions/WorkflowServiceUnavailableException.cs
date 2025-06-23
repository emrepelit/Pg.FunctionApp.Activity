using System.Net;

namespace Pg.FunctionApp.Activity.Core.Exceptions;

public class WorkflowServiceUnavailableException : Exception
{
    public WorkflowServiceUnavailableException(string message, HttpStatusCode statusCode, int retryAttempts)
        : base(message)
    {
        StatusCode = statusCode;
        RetryAttempts = retryAttempts;
    }

    public HttpStatusCode StatusCode { get; }
    public int RetryAttempts { get; }
}