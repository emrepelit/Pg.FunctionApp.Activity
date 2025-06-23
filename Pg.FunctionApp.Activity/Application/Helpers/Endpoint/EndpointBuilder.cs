namespace Pg.FunctionApp.Activity.Application.Helpers.Endpoint;

public static class EndpointBuilder
{
    private const string Domain = "www.x.ai";
    private const string HttpsPrefix = "https://";
    private const string MethodGet = "get-workflow";
    private const string MethodPost = "feed-dataflow";

    public static string BuildGetWorkflowEndpoint(string customerId, string property)
    {
        //I did not used "HttpsPrefix" below to match it with the doc.
        return Domain + "/" + MethodGet + "/" + nameof(customerId) + "=" + customerId + "&" + nameof(property) + "=" +
               property;
    }

    public static string BuildPostWorkflowEndpoint()
    {
        return HttpsPrefix + Domain + "/" + MethodPost;
    }
}