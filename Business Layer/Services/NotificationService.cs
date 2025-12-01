namespace Business_Layer.Services;

public class NotificationService(IMapper mapper, AppDbContext context) : INotificationService
{
    public async Task<Response<List<GetNotificationDto>>> GetAllNotifications(int userId)
    {
        var response = new Response<List<GetNotificationDto>>();

        var notifications = context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.Id);

        response.Result = mapper.Map<List<GetNotificationDto>>(notifications.ToList());

        var notReadingNotifications = await notifications.Where(n => n.ReadAtDateTime == null).ToListAsync();

        foreach (var notReadingNotification in notReadingNotifications)
        {
            notReadingNotification.ReadAtDateTime = DateTime.Now;
        }

        await context.SaveChangesAsync();

        response.Success = true;

        return response;
    }

    public async Task<Response<GetNotificationDto>> Add(GetNotificationDto dto)
    {
        var response = new Response<GetNotificationDto>();

        try
        {
            var notification = mapper.Map<Notification>(dto);

            notification = (await context.Notifications.AddAsync(notification)).Entity;

            await context.SaveChangesAsync();

            response.Result = mapper.Map<GetNotificationDto>(notification);

            response.Success = true;
        }
        catch (Exception e)
        {
            response.Error = e.Message;
        }

        return response;
    }
}