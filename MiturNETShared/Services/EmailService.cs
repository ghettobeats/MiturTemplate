using System.Net.Mail;
using System.Net;

namespace MiturNetShared.Services;

public class EmailService : IEmailService
{
    private MiturNetShared.Helper.EmailSettings _mailConfig;
    public EmailService(MiturNetShared.Helper.EmailSettings mailConfig)
    {
        _mailConfig = mailConfig;
    }

    public async Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
    {
        MailMessage message = new MailMessage();
        SmtpClient smtp = new SmtpClient();
        message.From = new MailAddress(_mailConfig.FromEmail);
        message.To.Add(new MailAddress(ToEmail));
        message.Subject = Subject;
        message.IsBodyHtml = true;
        message.Body = HTMLBody;
        smtp.Port = _mailConfig.Port;
        smtp.Host = _mailConfig.Host;
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_mailConfig.Username, _mailConfig.Password);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

        await smtp.SendMailAsync(message);
    }
}