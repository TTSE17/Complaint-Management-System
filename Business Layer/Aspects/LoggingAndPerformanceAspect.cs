using System.Diagnostics;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace Cross_Cutting_Layer.Aspects;

public class LoggingAndPerformanceAspect : AbstractInterceptorAttribute
{
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        // Get logger from dependency injection container
        var logger = (ILogger<LoggingAndPerformanceAspect>)context.ServiceProvider
            .GetService(typeof(ILogger<LoggingAndPerformanceAspect>))!;

        // Get method name (for better log messages)
        var methodName = $"{context.ImplementationMethod.DeclaringType?.FullName}.{context.ImplementationMethod.Name}";

        // Start timer
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation($"➡️ Start {methodName}");

        try
        {
            // Call the original method
            await next(context);

            stopwatch.Stop();
            logger.LogInformation($"✅ {methodName} finished in {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            // Log and rethrow exception
            logger.LogError(ex, $"❌ Error in {methodName}");
            throw;
        }
    }
}