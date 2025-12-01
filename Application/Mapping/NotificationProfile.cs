namespace Application.Mapping;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, GetNotificationDto>().ReverseMap();
    }
}