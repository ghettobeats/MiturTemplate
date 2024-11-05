namespace MiturNetApplication.Extensions;
public static class UrlHelperExtensions
{
    public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
    {
        return scheme + "/confirm-email?userId=" + userId + "&code=" + code;
    }

    public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
    {
        return scheme + "/reset-password?userId=" + userId + "&code=" + code;
    }
}
