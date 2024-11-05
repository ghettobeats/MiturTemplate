namespace MiturNetShared;
public class HttpClientOptions
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string AuthorizationCode { get; set; }
    public string AccessToken { get; set; }
    public string BaseUrl { get; set; }
    public string URL { get; set; }

    public HttpClientOptions()
    {
        this.ClientId = "";
        this.ClientSecret = "";
        this.AuthorizationCode = "";
        this.AccessToken = ""; 
        this.URL = "";
    }
}
