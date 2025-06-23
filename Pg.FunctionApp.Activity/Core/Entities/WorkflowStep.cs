namespace Pg.FunctionApp.Activity.Core.Entities;

public class WorkflowStep
{
    public int Id { get; set; }
    public string StepName { get; set; } = null!;
    public int Weight { get; set; }
    public long DelayTimeInMs { get; set; }
}