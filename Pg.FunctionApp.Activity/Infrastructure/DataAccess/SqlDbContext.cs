using Microsoft.EntityFrameworkCore;
using Pg.FunctionApp.Activity.Core.Entities;

namespace Pg.FunctionApp.Activity.Infrastructure.DataAccess;

public class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
    {
    }

    public DbSet<Audit> Audits { get; set; }
    public DbSet<WorkflowStep> WorkflowSteps { get; set; }
}