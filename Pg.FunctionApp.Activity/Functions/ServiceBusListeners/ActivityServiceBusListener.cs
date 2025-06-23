using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pg.FunctionApp.Activity.Application.Handlers.Message.Abstract;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Functions.ServiceBusListeners;

public class ActivityServiceBusListener
{
    private readonly ILogger<ActivityServiceBusListener> _logger;
    private readonly IActivityHandler _activityHandler;

    public ActivityServiceBusListener(ILogger<ActivityServiceBusListener> logger, IActivityHandler activityHandler)
    {
        _logger = logger;
        _activityHandler = activityHandler;
    }

    [Function(nameof(ActivityServiceBusListener))]
    public async Task Run(
        [ServiceBusTrigger("activity", "PgSubscription", Connection = "PgConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            var jsonString = message.Body.ToString();
            var baseMessage = JsonConvert.DeserializeObject<JObject>(jsonString);
            var messageType = baseMessage?["type"]?.ToString();
            
            ActivityMessageBase? activityMessage = messageType switch
            {
                "dashboard_entries" => JsonConvert.DeserializeObject<DashboardActivityMessage>(jsonString),
                "crm" => JsonConvert.DeserializeObject<CrmActivityMessage>(jsonString),
                _ => throw new InvalidOperationException($"This message type is not supported= {messageType}")
            };

            if (activityMessage != null) 
            {
                await _activityHandler.Handle(activityMessage);
            
                await messageActions.CompleteMessageAsync(message);
            }
            else
            {
                _logger.LogError($"Error while deserialising the message for MessageId= {message.MessageId}");
            
                await messageActions.DeadLetterMessageAsync(message,
                    (Dictionary<string, object>?)message.ApplicationProperties,
                    "Error while deserialising the message to ActivityServiceBusMessage.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occured while processing message with MessageId= {message.MessageId}");
            
            await messageActions.AbandonMessageAsync(message);
            throw;
        }
    }
}