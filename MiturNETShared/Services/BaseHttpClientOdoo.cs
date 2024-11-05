using Microsoft.AspNetCore.Http;

namespace MiturNetShared.Services;
public class BaseHttpClientOdoo : IBaseHttpClientOdoo
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;
    private readonly LoginOdoo OdooOptions;

    public BaseHttpClientOdoo(
        HttpClient http,
        IOptions<LoginOdoo> OdooOptions,
        ILocalStorageService localStorage
        )
    {
        _localStorage = localStorage;
        _http = http;
        this.OdooOptions = OdooOptions.Value;
        _http.Timeout = new TimeSpan(0, 120, 0);

        // Set values for the client
        _http.BaseAddress = new Uri(this.OdooOptions.Url);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void SetupToken(string token)
    {
        _http.DefaultRequestHeaders.Add("access-token", token);
    }

    public async Task<T> MakeRequest<T>(string httpMethod, string route, Dictionary<string, string> postParams = null, CancellationToken cancellationToken = default)
    {
        T res = (T)Activator.CreateInstance(typeof(T));

        HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(httpMethod), $"{this.OdooOptions.Url}/{route}");

        if (postParams != null)
            requestMessage.Content = new FormUrlEncodedContent(postParams);

        try
        {
            HttpResponseMessage response = await _http.SendAsync(requestMessage, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                res = (response.Content.ReadFromJsonAsync<T>()).Result;
                //res = response.Content.ReadFromJsonAsync<T>().Result;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return res;
    }
}
