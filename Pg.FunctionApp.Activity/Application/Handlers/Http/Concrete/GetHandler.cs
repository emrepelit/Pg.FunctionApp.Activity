using Microsoft.Extensions.Logging;
using Pg.FunctionApp.Activity.Application.Handlers.Http.Abstract;
using Pg.FunctionApp.Activity.Application.Helpers.Endpoint;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.Apis;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Application.Handlers.Http.Concrete;

public class GetHandler : BaseHttpHandler<GetHandler>, IGetHandler
{
    public GetHandler(HttpClient httpClient, ILogger<GetHandler> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<GetWorkflowResponseModel?> GetWorkflowAsync(ActivityMessageBase activityMessageBase)
    {
        var getEndpoint = EndpointBuilder
            .BuildGetWorkflowEndpoint(
                activityMessageBase.GetCustomerIdValue(),
                activityMessageBase.GetPropertyValue());

        return await SendGetRequestAsync<GetWorkflowResponseModel>(
            getEndpoint);
    }
}