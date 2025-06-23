using Pg.FunctionApp.Activity.Infrastructure.Dtos.Apis;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Application.Handlers.Http.Abstract;

public interface IGetHandler
{
    Task<GetWorkflowResponseModel?> GetWorkflowAsync(ActivityMessageBase activityMessageBase);
}