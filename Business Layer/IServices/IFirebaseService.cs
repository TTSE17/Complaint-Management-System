namespace Business_Layer.IServices;

public interface IFirebaseService
{
    Task<Response<string>> SendNotificationAsync(string deviceToken, string title, string body);

    // Task NotifyAdmin(string title, string body);

    Task NotifyUser(string title, string body, int userId);

    // Task NotifyWorkers(string title, string body, int? buffetId = null);
}