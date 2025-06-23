using System.Net;

namespace Pg.FunctionApp.Activity.Core.Exceptions;

public class WorkflowInvalidResponseException : Exception
{
    public WorkflowInvalidResponseException(
        string message,
        HttpStatusCode statusCode,
        string? responseContent = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseContent = responseContent;
    }

    public HttpStatusCode StatusCode { get; }
    public string? ResponseContent { get; }
}