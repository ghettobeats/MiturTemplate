namespace MiturNetWeb.Pages;
public partial class Index : BaseComponentInject
{
    protected override async Task OnInitializedAsync()
    {
        var auth = (await _authentication).User.Identity.IsAuthenticated;
        if (auth)
        {
            await _securityService.getAccessToken();
            UriHelper.NavigateTo("/dashboard");
        }
        else
        {
            UriHelper.NavigateTo("/login");
        }
    }
}
