namespace Business_Layer.IServices;

public interface ITraceLogService
{
    Task AddAsync(TraceLog log);
}