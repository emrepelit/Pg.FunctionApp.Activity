namespace Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

public class CrmActivityMessage : ActivityMessageBase
{
    public string Type { get; set; }
    public string Operation { get; set; }
    public string Date { get; set; }
    public PayloadCrm Payload { get; set; }

    public override string GetCustomerIdValue() => Payload.CustomerId;
    public override string GetPropertyValue() => Payload.Property;
    
    public class PayloadCrm
    {
        public string CustomerId { get; set; }
        public string Property { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Required { get; set; }
    }
}