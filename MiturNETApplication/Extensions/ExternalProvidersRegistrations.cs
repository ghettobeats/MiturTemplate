namespace MiturNetApplication.Extensions;
public static class ExternalProvidersRegistrations
{
    public static void ConfigureExternalProviders(this IServiceCollection services, IConfiguration configuration)
    {
        // Google

        //dotnet user-secrets set "Authentication:Google:ClientId" ""
        //dotnet user-secrets set "Authentication:Google:ClientSecret" ""
        /*
        if (configuration["Authentication:Google:ClientId"] != null)
        {
            services.AddAuthentication().AddGoogle(o =>
            {
                o.ClientId = configuration["Authentication:Google:ClientId"];
                o.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });
        }

        // Facebook

        // dotnet user-secrets set Authentication:Facebook:AppId ""
        // dotnet user-secrets set Authentication:Facebook:AppSecret ""

        if (configuration["Authentication:Facebook:AppId"] != null)
        {
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            });
        }
        */
    }
}
