using ADIA.OpenAi.Proxy.Config.Models;
using ADIA.OpenAi.Proxy.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ADIA.OpenAi.Proxy.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenIaConfig(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton(new OpenIaConfig()
        {
            ApiKey = config["OpenIaConfig:ApiKey"] ?? throw new ArgumentNullException("OpenIaConfig:ApiKey")
        });

        services.AddTransient<IAnalysisTextOpenAI, AnalysisTextOpenAI>();
        services.AddTransient<IAnalysisImageOpenAIService, AnalysisImageOpenAIService>();

        return services;
    }
}