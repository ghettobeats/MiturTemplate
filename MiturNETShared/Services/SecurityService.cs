using Microsoft.Extensions.Options;

namespace MiturNetShared.Services;
public class SecurityService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IBaseHttpClient _baseHttpClient;
    private HttpClientConfig _options;

    public string localToken { get; set; }

    public SecurityService(
            AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage,
        IBaseHttpClient baseHttpClient,
        HttpClientConfig options
        )
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _baseHttpClient = baseHttpClient;
        _options = options;

    }
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("AuthToken");
        await _localStorage.RemoveItemAsync("AuthTokenOdoo");
        
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
    }
    public async Task<Response<AccountLoginResult>> Login(AccountLogin login)
    {
        Response<AccountLoginResult> response = new();
        try
        {
            response = await _baseHttpClient.Add<AccountLoginResult>(login, "Account/login");
            if (!response.Succes)
            {
                return response;
            }
            await _localStorage.SetItemAsync("AuthToken", response.Data.token);
            var localToken = await _localStorage.GetItemAsync<string>("AuthToken");

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(localToken);
            _options.AccessToken = localToken;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Succes = false;
        }

        return response;
    }

    public async Task<bool> Refresh() => (await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
        .GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;

    public ClaimsPrincipal Principal
    {
        get
        {
            var usu = ((ApiAuthenticationStateProvider)_authenticationStateProvider).GetAuthenticationStateAsync().Result.User;
            //return _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
            return usu;
        }
    }

    public bool IsAuthenticated()
    {
        return Principal.Identity.IsAuthenticated;
    }

    public bool IsInRole(params string[] roles)
    {
        bool isValid = ((ApiAuthenticationStateProvider)_authenticationStateProvider).IsInRole(roles).Result;

        return isValid;
    }

    //public async Task<bool> IsInRole(params string[] roles)
    //{


    //    //var Usuario = ((ApiAuthenticationStateProvider)_authenticationStateProvider).GetAuthenticationStateAsync().Result.User;
    //    //if (roles.Contains("Everybody"))
    //    //{
    //    //Visible = "@SecurityService.IsInRole(new string[]{"Administrador","Operaciones"})"
    //    //Visible = "@SecurityService.IsInRole(new string[]{"Administrator","Roles"})"
    //    //    return true;
    //    //}

    //    //if (!IsAuthenticated())
    //    //{
    //    //    return false;
    //    //}

    //    //if (roles.Contains("Administrador"))
    //    //{
    //    //    return true;
    //    //}
    //    //return false;

    //    //if (((ApiAuthenticationStateProvider)_authenticationStateProvider).GetAuthenticationStateAsync().Result.User.IsInRole("Administrator"))
    //    //{
    //    //    // Execute Admin logic
    //    //}

    //    // new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt");

    //    return false;

    //    //var losRoles = roles.Any(role => ((ApiAuthenticationStateProvider)_authenticationStateProvider).GetAuthenticationStateAsync().Result.User.IsInRole(role));
    //    //return losRoles;
    //    //return roles.Any(role => Principal.IsInRole(role));
    //}

    public async Task getAccessToken()
    {
        var localToken = await _localStorage.GetItemAsync<string>("AuthToken");
        _options.AccessToken = localToken;
    }  
    
    //public async Task<SPValidarFlujo> ValidarFlujo(string usuario)
    //{
    //    Response<IEnumerable<SPValidarFlujo>> datos;
        
    //    datos = await _baseHttpClient.Get<IEnumerable<MiturNetShared.Model.Operation.SPValidarFlujo>>($"AprobacionValidarFlujo/ValidarFlujo/{usuario}");        
        
    //    return datos.Data.FirstOrDefault();
    //}
}

