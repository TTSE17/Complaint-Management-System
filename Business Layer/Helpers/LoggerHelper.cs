using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Helpers;
public class LoggerHelper
{
    private readonly ITraceLogService _traceService;

    public LoggerHelper(ITraceLogService traceService)
    {
        _traceService = traceService;
    }

    public async Task Log(
        int? userId,
        string? userName,
        string action,
        bool isSuccess,
        string? exception = null)
    {
        var log = new TraceLog
        {
            UserId = userId,
            UserName = userName,
            Action = action,
            IsSuccess = isSuccess,
            Exception = exception
        };

        await _traceService.AddAsync(log);
    }
}