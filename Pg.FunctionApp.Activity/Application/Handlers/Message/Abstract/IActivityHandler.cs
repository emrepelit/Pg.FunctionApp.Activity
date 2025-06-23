using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Application.Handlers.Message.Abstract;

public interface IActivityHandler
{
    Task Handle(ActivityMessageBase activityMessageBase);
}