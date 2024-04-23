using ADIA.Uow.SqlServer.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ADIA.Uow.SqlServer.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlServerDB(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("AppContext");

        services.AddDbContext<AppDbContext>(m =>
            m.UseSqlServer(connectionString, e =>
            {
                //e.EnableRetryOnFailure();
                //e.MigrationsAssembly(typeof(T).Assembly.FullName);
                //e.MigrationsHistoryTable("__EFMigrationsHistory", "Inc");
            }));

        //services.AddTransient<IDbConnection>(db => new SqlConnection(connectionString));

        // migrate any database changes on startup (includes initial db creation)
        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();


        return services;
    }
}