namespace ADIA.Web.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UnhandledExceptionMiddleware> _logger;

    public UnhandledExceptionMiddleware(RequestDelegate next, ILogger<UnhandledExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Se produjo una excepción no controlada: {ex.Message}", ex);
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync("Se ha producido un error interno del servidor.");
            }
        }
    }
}