namespace Business_Layer.IServices;

public interface INotificationService
{
    Task<Response<List<GetNotificationDto>>> GetAllNotifications(int userId);

    Task<Response<GetNotificationDto>> Add(GetNotificationDto dto);
}