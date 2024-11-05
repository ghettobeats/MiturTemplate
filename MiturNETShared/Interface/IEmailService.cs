namespace MiturNetShared.Interface;

public interface IEmailService
{
    Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
}
