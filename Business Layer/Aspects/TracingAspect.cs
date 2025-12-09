using System.Security.Claims;
using AspectCore.DynamicProxy;

namespace Business_Layer.Aspects;

public class TracingAspect(string action) : AbstractInterceptorAttribute
{
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var serviceProvider = context.ServiceProvider;

        var traceLogService = serviceProvider.GetService<ITraceLogService>()!;

        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

        var log = new TraceLog
        {
            Action = action,
            Timestamp = DateTime.Now
        };

        try
        {
            await next(context);

            var returnValue = context.ReturnValue;

            var response = await GetResult(returnValue);

            log.IsSuccess = response.Success;

            log.Exception = response.Error?.ToString();

            // var user = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
            var user = httpContextAccessor?.HttpContext?.User;

            if (user != null)
            {
                log.UserId = Convert.ToInt32(user.FindFirstValue(ClaimTypes.NameIdentifier));

                log.UserName = user.FindFirstValue(ClaimTypes.Name);
            }
        }
        catch (Exception ex)
        {
            log.IsSuccess = false;

            log.Exception = ex.Message;
        }
        finally
        {
            await traceLogService.AddAsync(log);
        }
    }

    private static async Task<Response<int>> GetResult(object result)
    {
        if (result is Task task)
            await task;

        var response = new Response<int>
        {
            Success = true
        };

        // Check if it's Response<T>
        if (!result.GetType().IsGenericType)
        {
            // return type.Name; // Just return type name for other types

            return response;
        }

        // Get result from Task<T>
        var resultProperty = result.GetType().GetProperty("Result");

        if (resultProperty == null) return response;

        var actualResult = resultProperty.GetValue(result);

        var type = resultProperty.GetValue(result)?.GetType();

        if (type?.GetGenericTypeDefinition() != typeof(Response<>)) return response;

        var isSuccess = type.GetProperty("Success")?.GetValue(actualResult);

        response.Success = (bool)(isSuccess ?? false);

        var error = type.GetProperty("Error")?.GetValue(actualResult);

        response.Error = error;

        return response;
    }
}