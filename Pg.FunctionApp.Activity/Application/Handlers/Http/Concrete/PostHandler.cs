using Microsoft.Extensions.Logging;
using Pg.FunctionApp.Activity.Application.Handlers.Http.Abstract;
using Pg.FunctionApp.Activity.Application.Helpers.Endpoint;
using Pg.FunctionApp.Activity.Core.Entities;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Application.Handlers.Http.Concrete;

public class PostHandler : BaseHttpHandler<PostHandler>, IPostHandler
{
    public PostHandler(HttpClient httpClient, ILogger<PostHandler> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<HttpResponseMessage> PostWorkflowAsync(List<WorkflowStep> workflowSteps,
        ActivityMessageBase activityMessageBase, string workflowId)
    {
        var postEndpoint = EndpointBuilder.BuildPostWorkflowEndpoint();
        
        var postData = BuildPostObject(workflowSteps, activityMessageBase, workflowId);

        return await SendPostRequestAsync(postEndpoint, postData);
    }

    private static object BuildPostObject(List<WorkflowStep> workflowSteps,
        ActivityMessageBase activityMessageBase, string workflowId)
    {
        
        var postData = new 
        {
            CustomerId = activityMessageBase.GetCustomerIdValue(),
            Property = activityMessageBase.GetPropertyValue(),
            _id = workflowId,
            BusinessId = 1,
            Steps = workflowSteps.OrderBy(s => s.StepName)
                .Select(s => new 
                {
                    s.Id,
                    s.StepName,
                    s.Weight,
                    s.DelayTimeInMs
                })
        };
        
        return postData;
    }
}