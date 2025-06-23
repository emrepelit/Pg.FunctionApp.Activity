using Pg.FunctionApp.Activity.Core.Entities;

namespace Pg.FunctionApp.Activity.Infrastructure.DataAccess.Repositories.Abstract;

public interface INetWorkflowStepRepository
{
    Task<List<WorkflowStep>> InsertAuditAndSelectWorkflowStepsAsync(string id, int businessId = 1);
}