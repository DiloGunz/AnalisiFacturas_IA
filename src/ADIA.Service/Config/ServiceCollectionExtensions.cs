using ADIA.AzureIa.Proxy.Config;
using ADIA.OpenAi.Proxy.Config;
using ADIA.Service.AnalysisStrategies;
using ADIA.Service.AnalysisStrategies.Interfaces;
using ADIA.Uow.Config;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using static ADIA.Shared.Enums.EntityEnums;

namespace ADIA.Service.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceConfig(this IServiceCollection services, IConfiguration config)
    {
        string ns = "ADIA.Service";

        services.AddUnitOfWork(config);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load(ns)));
        services.AddAutoMapper(Assembly.Load(ns));
        services.AddValidatorsFromAssembly(Assembly.Load(ns));

        services.AddOpenIaConfig(config);
        services.AddAzureIaConfig(config);

        services.AddSingleton<ImageAnalysisStrategy>();
        services.AddSingleton<PdfAnalysisStrategy>();

        services.AddSingleton<IAnalysisStrategyResolver, AnalysisStrategyResolver>(serviceProvider =>
        {
            var strategies = new Dictionary<FileType, IAnalysisStrategy>
            {
                { FileType.Image, serviceProvider.GetRequiredService<ImageAnalysisStrategy>() },
                { FileType.Pdf, serviceProvider.GetRequiredService<PdfAnalysisStrategy>() }
            };
            return new AnalysisStrategyResolver(strategies);
        });

        return services;
    }
}