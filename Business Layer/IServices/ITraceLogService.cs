using Microsoft.AspNetCore.Mvc;

namespace Business_Layer.IServices;

public interface ITraceLogService
{
    Task AddAsync(TraceLog log);
}
