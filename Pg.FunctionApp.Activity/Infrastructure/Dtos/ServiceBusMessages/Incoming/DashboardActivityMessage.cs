using System.Text.Json.Serialization;

namespace Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

public class DashboardActivityMessage : ActivityMessageBase
{
    public PayloadDashboard Payload { get; set; }

    public string Type { get; set; }
    public string Date { get; set; }
    [JsonPropertyName("__v")]
    public int V { get; set; }

    public override string GetCustomerIdValue() => Payload.CustomerId;
    public override string GetPropertyValue() => Payload.Property;
    
    public class PayloadDashboard
    {
        public string CustomerId { get; set; }
        public string Property { get; set; }
        public string Details { get; set; }
    }
}