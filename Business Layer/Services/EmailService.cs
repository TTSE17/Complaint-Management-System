using System.Net;
using System.Net.Mail;

namespace Business_Layer.Services;

public class EmailService : IEmailService
{
    public async Task<Response<bool>> SendEmail(string to, string subject, string body)
    {
        var response = new Response<bool>();

        const string fromAddress = "notifications.tt.app@gmail.com";

        const string fromPassword = "kboh deco lmvf jpaw"; // Use an app-specific password if 2FA is enabled

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromAddress, fromPassword),
            EnableSsl = true
        };

        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true, // Set to true if you want to send HTML content
            };

            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);

            response.Success = true;

            return response;
        }
        catch (Exception ex)
        {
            response.Error = "An error occurred : " + ex;

            return response;
        }
    }
}