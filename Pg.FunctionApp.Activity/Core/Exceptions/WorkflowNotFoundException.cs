using System.Net;

namespace Pg.FunctionApp.Activity.Core.Exceptions
{
    public class WorkflowNotFoundException : Exception
    {
        public WorkflowNotFoundException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}