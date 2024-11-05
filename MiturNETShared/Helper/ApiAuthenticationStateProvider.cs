namespace MiturNetShared.Helper;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    //private readonly ILocalStorageService _localStorage;
    private ILocalStorageService _localStorage { get; }
    private IEnumerable<Claim> _claims;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("AuthToken");

        if (string.IsNullOrWhiteSpace(savedToken))        
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        //v2 new line

        _claims  = ParseClaimsFromJwt(savedToken);

        var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(_claims.Where(c => c.Type.Equals("exp")).FirstOrDefault().Value));
        if (exp.Equals(null))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        if(exp.UtcDateTime<= DateTime.UtcNow)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
    //end v2
    }
    public void MarkUserAsAuthenticated(string token)
    {
        var authenticateUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
        var authState = Task.FromResult(new AuthenticationState(authenticateUser));
        NotifyAuthenticationStateChanged(authState);
    }
    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public ClaimsIdentity GetClaimsIdentity(UserSession user)
    {
        var claimsIdentity = new ClaimsIdentity();

        if (user.UserId != null)
        {
            claimsIdentity = new ClaimsIdentity(new[]
                            {
                                    new Claim(ClaimTypes.Name, user.UserName),
                                    new Claim(ClaimTypes.Role, user.Role)                                   
                                }, "jwt");
        }

        return claimsIdentity;
    }

    public async Task<bool> IsInRole(params string[] roles)
    {
        var savedTokenClaims = await _localStorage.GetItemAsync<string>("AuthToken");

       var  _claimsRoles = ParseClaimsFromJwt(savedTokenClaims);
       
        return true;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parseRoles = System.Text.Json.JsonSerializer.Deserialize<string[]>(roles.ToString());
                foreach (var role in parseRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }
            keyValuePairs.Remove(ClaimTypes.Role);
        }
        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
        return claims;
    }
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}

