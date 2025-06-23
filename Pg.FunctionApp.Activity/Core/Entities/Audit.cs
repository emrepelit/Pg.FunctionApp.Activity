namespace Pg.FunctionApp.Activity.Core.Entities;

public class Audit
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public DateTime DateTime { get; set; }
}