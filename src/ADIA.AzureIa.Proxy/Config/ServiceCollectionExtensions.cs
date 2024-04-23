using ADIA.AzureIa.Proxy.Config.Models;
using ADIA.AzureIa.Proxy.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ADIA.AzureIa.Proxy.Config;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Agrega la configuracion de dependencias para la liberia de clases 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAzureIaConfig(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton(new AzureIaConfig()
        {
            ApiKey = config["AzureIaConfig:ApiKey"] ?? throw new ArgumentNullException("OpenIaConfig:ApiKey"),
            EndPoint = config["AzureIaConfig:EndPoint"] ?? throw new ArgumentNullException("OpenIaConfig:EndPoint")
        });

        services.AddTransient<IAnalysisPdfAzureAIService, AnalysisPdfAzureAIService>();

        return services;
    }
}