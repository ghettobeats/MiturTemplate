namespace MiturNetApplication.Extensions;

public static class EmailSenderExtensions
{
    public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string message)
    {
        return emailSender.SendEmailAsync(email, "Confirmar su correo", message);
    }
}
