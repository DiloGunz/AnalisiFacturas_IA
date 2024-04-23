using ADIA.Uow.Interfaces;
using ADIA.Uow.Repository;
using ADIA.Uow.SqlServer.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ADIA.Uow.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration config)
    {
        services.AddSqlServerDB(config);//.AddScoped(p => p.GetService<ApplicationDbContext>()!);

        services.AddTransient<IUnitOfWork, UnitOfWorkContainer>();
        services.AddTransient<IUnitOfWorkRepository, UnitOfWorkRepository>();

        return services;
    }
}