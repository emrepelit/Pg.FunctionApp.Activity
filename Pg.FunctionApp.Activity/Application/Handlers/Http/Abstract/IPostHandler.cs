using Pg.FunctionApp.Activity.Core.Entities;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Application.Handlers.Http.Abstract;

public interface IPostHandler
{
    Task<HttpResponseMessage> PostWorkflowAsync(List<WorkflowStep> workflowSteps,
        ActivityMessageBase activityMessageBase, string workflowId);
}