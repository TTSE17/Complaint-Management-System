namespace Business_Layer.IServices;

public interface IEmailService
{
    Task<Response<bool>> SendEmail(string to, string subject, string body);
}