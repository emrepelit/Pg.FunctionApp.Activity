using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pg.FunctionApp.Activity.Application.Handlers.Http.Abstract;
using Pg.FunctionApp.Activity.Application.Handlers.Http.Concrete;
using Pg.FunctionApp.Activity.Application.Handlers.Message.Abstract;
using Pg.FunctionApp.Activity.Application.Handlers.Message.Concrete;
using Pg.FunctionApp.Activity.Infrastructure.DataAccess;
using Pg.FunctionApp.Activity.Infrastructure.DataAccess.Repositories.Abstract;
using Pg.FunctionApp.Activity.Infrastructure.DataAccess.Repositories.Concrete;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton(_ =>
        {
            var connectionString = context.Configuration.GetConnectionString("SqlConnection");
            return new SqlConnection(connectionString);
        });
        services.AddDbContext<SqlDbContext>(options => 
            options.UseInMemoryDatabase("PgInMemoryDb"));
        services.AddScoped<INetWorkflowStepRepository, NetWorkflowStepRepository>();
        services.AddScoped<IGetHandler, GetHandler>();
        services.AddScoped<IPostHandler, PostHandler>();
        services.AddScoped<IActivityHandler, ActivityHandler>();
    })
    .Build();

builder.Run();