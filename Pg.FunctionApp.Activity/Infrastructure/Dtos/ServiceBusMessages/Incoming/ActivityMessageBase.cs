namespace Pg.FunctionApp.Activity.Infrastructure.Dtos.ServiceBusMessages.Incoming;

public abstract class ActivityMessageBase
{
    public abstract string GetCustomerIdValue();
    public abstract string GetPropertyValue();
    
    public string Type { get; set; }
    public DateTime Date { get; set; }
}