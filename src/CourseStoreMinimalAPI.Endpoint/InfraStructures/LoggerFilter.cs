using FluentValidation;

namespace CourseStoreMinimalAPI.Endpoint.InfraStructures;
public class LoggerFilter : IEndpointFilter
{
    private readonly ILogger logger;

    public LoggerFilter(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<LoggerFilter>()  ;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string path = context.HttpContext.Request.Path;

        logger.LogInformation("start", path, DateTime.UtcNow);
        var result = await next(context);
        logger.LogInformation("end", path, DateTime.UtcNow);

        return result;
    }
}

