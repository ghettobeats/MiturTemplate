namespace MiturNetWeb.Shared;
public partial class LoginRedirect : BaseComponentInject
{
    protected override void OnInitialized()
    {
        UriHelper.NavigateTo("login", true);
    }
}

