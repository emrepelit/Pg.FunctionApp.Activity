using System.Net;
using System.Text.Json;
using Pg.FunctionApp.Activity.Application.Handlers.Http.Abstract;
using Pg.FunctionApp.Activity.Application.Handlers.Message.Abstract;
using Pg.FunctionApp.Activity.Core.Exceptions;
using Pg.FunctionApp.Activity.Infrastructure.DataAccess.Repositories.Abstract;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Application.Handlers.Message.Concrete;

public class ActivityHandler : IActivityHandler
{
    private readonly IGetHandler _getHandler;
    private readonly IPostHandler _postHandler;
    private readonly INetWorkflowStepRepository _netWorkflowStepRepository;

    public ActivityHandler(
        IGetHandler getHandler,
        IPostHandler postHandler,
        INetWorkflowStepRepository netWorkflowStepRepository)
    {
        _getHandler = getHandler;
        _postHandler = postHandler;
        _netWorkflowStepRepository = netWorkflowStepRepository;
    }

    public async Task Handle(ActivityMessageBase activityMessageBase)
    {
        var workflow = await _getHandler.GetWorkflowAsync(activityMessageBase);
        
        if (string.IsNullOrEmpty(workflow?.Id))
        {
            throw new WorkflowInvalidResponseException(
                "API returned 200, however, data was invalid= Workflow Id can not be null or empty." +
                $"CustomerId= {activityMessageBase.GetCustomerIdValue()}," +
                $"Property= {activityMessageBase.GetPropertyValue()}",
                HttpStatusCode.NoContent,
                JsonSerializer.Serialize(activityMessageBase));
        }

        var workflowSteps = await _netWorkflowStepRepository
            .InsertAuditAndSelectWorkflowStepsAsync(workflow.Id);

        await _postHandler.PostWorkflowAsync(workflowSteps, activityMessageBase, workflow.Id);
    }
}