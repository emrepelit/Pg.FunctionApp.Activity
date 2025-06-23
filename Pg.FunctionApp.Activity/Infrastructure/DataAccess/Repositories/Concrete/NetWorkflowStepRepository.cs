using Microsoft.EntityFrameworkCore;
using Pg.FunctionApp.Activity.Core.Entities;
using Pg.FunctionApp.Activity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Pg.FunctionApp.Activity.Infrastructure.DataAccess.Repositories.Concrete;

public class NetWorkflowStepRepository : INetWorkflowStepRepository
{
    private readonly SqlDbContext _sqlDbContext;

    public NetWorkflowStepRepository(SqlDbContext sqlDbContext)
    {
        _sqlDbContext = sqlDbContext;
    }

    /// <summary>
    /// Inserts an audit record and selects workflow steps by executing [dbo].[GetNetWorkflowStep] stored procedure.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="businessId"></param>
    /// <returns></returns>
    public async Task<List<WorkflowStep>> InsertAuditAndSelectWorkflowStepsAsync(string id, int businessId)
    {
        /* While I was doing research about how to execute SP, I found out FromSqlInterpolated is
           safe against SQL injection=
          https://learn.microsoft.com/en-us/ef/core/querying/sql-queries?tabs=sqlserver */
        return await _sqlDbContext
            .Set<WorkflowStep>()
            .FromSqlInterpolated($"EXEC [dbo].[GetNetWorkflowStep] @ID={id}, @BusinessID={businessId}")
            .ToListAsync();
    }
}