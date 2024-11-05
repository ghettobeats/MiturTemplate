namespace MiturNetApplication.Services;
public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient(_emailSettings.MailServer);

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.Sender)
        };
        mailMessage.To.Add(email);
        mailMessage.Subject = subject;
        mailMessage.Body = message;
        mailMessage.IsBodyHtml = true;


        client.Port = _emailSettings.MailPort;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
        client.EnableSsl = _emailSettings.EnableSsl;
        client.SendMailAsync(mailMessage);

        return Task.CompletedTask;
    }
}
