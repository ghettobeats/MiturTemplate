using Microsoft.Extensions.Options;
using MiturNetShared.Model.Odoo;

namespace MiturNetShared.Services;
public class SecurityServiceOdoo
{
    private readonly ILocalStorageService _localStorage;
    private readonly IBaseHttpClientOdoo _baseHttpClient;
    private LoginOdoo _options;

    public string localToken { get; set; }

    public SecurityServiceOdoo(
        ILocalStorageService localStorage,
        IBaseHttpClientOdoo baseHttpClient,
        IOptions<LoginOdoo> options
        )
    {
        _localStorage = localStorage;
        _baseHttpClient = baseHttpClient;
        _options = options.Value;

    }
     public async Task LoginOdoo()
    {
        var localTokenOdoo = await _localStorage.GetItemAsync<string>("AuthTokenOdoo");
        if (string.IsNullOrWhiteSpace(localTokenOdoo))
        {
            var loginParams = new Dictionary<string, string> {
                { "login", _options.Login },
                { "password", _options.Password },
                { "db", _options.Db }
            };

            dynamic token = null;
            token = await _baseHttpClient.MakeRequest<OdooToken>("GET", "auth/token", loginParams);

            await _localStorage.SetItemAsync("AuthTokenOdoo", token.access_token);
        } else
        {
            await getAccessToken();
        }
    }

    public async Task getAccessToken()
    {
        var localToken = await _localStorage.GetItemAsync<string>("AuthTokenOdoo");
        _options.AccessToken = localToken;
    }



}

