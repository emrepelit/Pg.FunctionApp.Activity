using Azure.Messaging.ServiceBus;
using FakeItEasy;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pg.FunctionApp.Activity.Application.Handlers.Message.Abstract;
using Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

namespace Pg.FunctionApp.Activity.Test.Functions;

public class ActivityServiceBusListener
{
    private readonly IActivityHandler _activityHandler;
    private readonly Activity.Functions.ServiceBusListeners.ActivityServiceBusListener _underTest;

    public ActivityServiceBusListener()
    {
        ILogger<Activity.Functions.ServiceBusListeners.ActivityServiceBusListener> logger = A.Fake<ILogger<Activity.Functions.ServiceBusListeners.ActivityServiceBusListener>>();
        _activityHandler = A.Fake<IActivityHandler>();
        _underTest = new Activity.Functions.ServiceBusListeners.ActivityServiceBusListener(logger, _activityHandler);
    }

    [Fact]
    public async Task Should_ProcessDashboardMessage_Successfully()
    {
        // Arrange
        var message = CreateServiceBusMessage(
            new
            {
                type = "dashboard_entries",
                payload = new
                {
                    CustomerId = "68236ef4c8715b4c05c20a55",
                    Property = "email",
                    Details = "Test details"
                },
                date = "2025-05-13T16:10:28.536Z",
                __v = 0
            });
        
        var messageActions = A.Fake<ServiceBusMessageActions>();

        // Act
        await _underTest.Run(message, messageActions);

        // Assert
        A.CallTo(() => _activityHandler.Handle(A<DashboardActivityMessage>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => messageActions.CompleteMessageAsync(message, default))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_ProcessCrmMessageType_Successfully()
    {
        // Arrange
        var message = CreateServiceBusMessage(
            new
            {
                type = "crm",
                operation = "insert",
                payload = new
                {
                    CustomerId = "682370a8c5476986d6e00b3e",
                    Property = "MeetingRequest",
                    Date = "2025-10-11",
                    Time = "13:30",
                    Required = "Bobby, Sam, John"
                },
                date = "2025-05-13T16:17:44.061Z"
            });
        
        var messageActions = A.Fake<ServiceBusMessageActions>();

        // Act
        await _underTest.Run(message, messageActions);

        // Assert
        A.CallTo(() => _activityHandler.Handle(A<CrmActivityMessage>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => messageActions.CompleteMessageAsync(message, default))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_ThrowException_When_UnsupportedMessageType()
    {
        // Arrange
        var message = CreateServiceBusMessage(
            new
            {
                type = "emre",
                payload = new
                {
                    CustomerId = "9",
                    Property = "build"
                }
            });
        
        var messageActions = A.Fake<ServiceBusMessageActions>();

        // Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _underTest.Run(message, messageActions));
    }

    private static ServiceBusReceivedMessage CreateServiceBusMessage(object payload)
    {
        var jsonString = JsonConvert.SerializeObject(payload);
        
        return ServiceBusModelFactory.ServiceBusReceivedMessage(
            body: new BinaryData(jsonString),
            messageId: Guid.NewGuid().ToString());
    }
}