using ADIA.Service.Config;

namespace ADIA.Web.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
    {
        //services.AddFileLoggerProvider(config);
        services.AddServiceConfig(config);

        //services.AddScoped(
        //    typeof(IPipelineBehavior<,>),
        //    typeof(ValidationBehavior<,>)
        //);

        services.AddControllers(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        });

        //var secretKey = Encoding.ASCII.GetBytes(config.GetValue<string>("SecretKey") ?? string.Empty);

        return services;
    }
}