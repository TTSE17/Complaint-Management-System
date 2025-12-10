using Microsoft.AspNetCore.Hosting;

namespace Business_Layer.Services;

using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

public class FirebaseService : IFirebaseService
{
    private readonly AppDbContext _context;

    private readonly INotificationService _notificationService;

    public FirebaseService(AppDbContext context, IWebHostEnvironment env, INotificationService notificationService)
    {
        _context = context;

        _notificationService = notificationService;

        if (FirebaseApp.DefaultInstance != null) return;

        var basePath = env.ContentRootPath;
        var jsonPath = Path.Combine(basePath, "complaints-project-firebase.json");

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(jsonPath)
        });
    }

    public async Task<Response<string>> SendNotificationAsync(string deviceToken, string title, string body)
    {
        var response = new Response<string>();

        try
        {
            var message = new Message
            {
                Token = deviceToken,

                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            var messageId = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            response.Result = messageId;

            response.Success = true;
        }
        catch (Exception e)
        {
            response.Error = e.Message;
        }

        return response;
    }

    public async Task NotifyAdmin(string title, string body)
    {
        var admins = await _context.Users.AsNoTracking()
            .Where(u => u.Type == (int)UserType.Admin)
            .ToListAsync();

        foreach (var admin in admins)
        {
            if (admin.Fcm == null) continue;

            var response = await SendNotificationAsync(admin.Fcm, title, body);

            if (!response.Success) continue;

            var notificationDto = new GetNotificationDto
            {
                Title = title,
                Text = body,
                UserId = admin.Id,
                CreatedDateTime = DateTime.Now
            };

            await _notificationService.Add(notificationDto);
        }
    }

    public async Task NotifyUser(string title, string body, int userId)
    {
        var user = await _context.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user?.Fcm != null)
        {
            var response = await SendNotificationAsync(user.Fcm, title, body);

            if (!response.Success) return;

            var notificationDto = new GetNotificationDto
            {
                Title = title,
                Text = body,
                UserId = user.Id,
                CreatedDateTime = DateTime.Now
            };

            await _notificationService.Add(notificationDto);
        }
    }

    // public async Task NotifyWorkers(string title, string body, int? buffetId = null)
    // {
    //     var workers = await _context.Workers.AsNoTracking()
    //         .Where(w => w.BuffetId == buffetId || !buffetId.HasValue)
    //         .Include(w => w.User)
    //         .ToListAsync();
    //
    //     foreach (var worker in workers)
    //     {
    //         if (worker.User.FCM == null) continue;
    //
    // var response = await SendNotificationAsync(admin.Fcm, title, body);
    //
    //     if (!response.Success) continue;    //
    //         var notificationDto = new GetNotificationDto
    //         {
    //             Title = title,
    //             Text = body,
    //             UserId = worker.UserId,
    //             CreatedDateTime = DateTime.Now
    //         };
    //
    //         await _notificationService.Add(notificationDto);
    //     }
    // }
}