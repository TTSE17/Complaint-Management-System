namespace Business_Layer.Services;

public class TraceLogService(AppDbContext context) : ITraceLogService
{
    public async Task AddAsync(TraceLog log)
    {
        await context.TraceLogs.AddAsync(log);

        await context.SaveChangesAsync();
    }
}