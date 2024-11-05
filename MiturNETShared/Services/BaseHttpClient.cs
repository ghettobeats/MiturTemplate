using Microsoft.AspNetCore.Components;
using MiturNetShared.Interface;
using Radzen;
using System.Net;
using System.Net.Mail;

namespace MiturNetShared.Services;
public class BaseHttpClient : IBaseHttpClient
{
    private readonly HttpClient _http;
    private readonly HttpClientOptions options;
    //private readonly LoginOdoo OdooOptions;
    private IHttpClientFactory _httpClientFactory;
    private readonly NavigationManager navigationManager;
    private HttpClientConfig _httpClientConfig;
    private MiturNetShared.Helper.EmailSettings _mailConfig;

    public BaseHttpClient(
        HttpClient http,
        IOptions<HttpClientOptions> options,
         //IOptions<LoginOdoo> OdooOptions,
        IHttpClientFactory httpClientFactory,
        NavigationManager navigationManager,
        HttpClientConfig httpClientConfig,
        MiturNetShared.Helper.EmailSettings mailSettings)
    {
        // Set values for instance variables
        _mailConfig = mailSettings;

        _httpClientConfig = httpClientConfig;

        _http = http;
        _httpClientFactory = httpClientFactory;

        this.options = options.Value;
        //this.OdooOptions = OdooOptions.Value;
        this.navigationManager = navigationManager;

        _http.Timeout = new TimeSpan(0, 120, 0);

        // Set values for the client
        _http.BaseAddress = new Uri(this.options.BaseUrl);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpClientConfig.AccessToken);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void SetupToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //_httpClientConfig.AccessToken = token;
    }
    public async Task<Response<T>> Add<T>(object root, string uri)
    {
        Response<T> response = new();
        try
        {
            HttpResponseMessage httpMessage = await _http.PostAsJsonAsync(uri, root);
            if (httpMessage.IsSuccessStatusCode)
            {
                response = httpMessage.Content.ReadAsAsync<Response<T>>().Result;
            }
            else
            {
                response.Message = "error";
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Succes = false;
        }
        return response;
    }
    public async Task<Response<T>> Update<T>(object root, string uri)
    {
        Response<T> res = new();
        HttpResponseMessage Httpresponse = await _http.PutAsJsonAsync(uri, root);
        if (Httpresponse.IsSuccessStatusCode)
        {
            res.Data = Httpresponse.Content.ReadAsAsync<T>().Result;
            res.Succes = true;
        }
        else
        {
            res.Message = Httpresponse.RequestMessage.ToString();
            res.Succes = false;
        }
        return res;
    }
    public async Task<Response<bool>> Delete(string uri)
    {
        Response<bool> res = new();
        try
        {
            HttpResponseMessage response = await _http.DeleteAsync(uri);
            if (response.IsSuccessStatusCode == true)
            {
                res.Data = true;
                res.Message = "Deleted !";
                res.Succes = true;
            }
        }
        catch (Exception ex)
        {
            res.Message = ex.Message;
        }

        return res;
    }
    public async Task<Response<T>> Get<T>(string uri)
    {
        Response<T> res = new();
        try
        {
            HttpResponseMessage response = await _http.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                res = response.Content.ReadFromJsonAsync<Response<T>>().Result;
            }
        }
        catch (Exception ex)
        {
            res.Message = ex.Message;
            res.Succes = false;
        }

        return res;
    }
    public async Task<Response<T>> UploadFile<T>(Stream stream, string file_name, string uri)
    {
        // Create the response to return
        Response<T> res = new();
        // Send data as multipart/form-data content
        using (MultipartFormDataContent content = new MultipartFormDataContent())
        {
            // Add content
            content.Add(new StreamContent(stream), "file", file_name);
            try
            {
                // Get the response
                HttpResponseMessage response = await this._http.PostAsync(uri, content);
                // Check the status code for the response
                if (response.IsSuccessStatusCode == true)
                {
                    // Get the data
                    string data = await response.Content.ReadAsStringAsync();
                    // Deserialize the data
                    res.Data = JsonConvert.DeserializeObject<T>(data);
                    res.Succes = true;
                }
                else
                {
                    // Get string data
                    string data = await response.Content.ReadAsStringAsync();
                    // Add error data
                    res.Message = $"UploadFile: {uri}. {Regex.Unescape(data)}";
                }
            }
            catch (Exception ex)
            {
                // Add exception data
                res.Message = $"UploadFile: {uri}. {ex.ToString()}";
            }
        }
        // Return the response
        return res;
    }
    public async Task<Response<bool>> DownloadFile(Stream stream, string uri)
    {
        // Create the response to return
        Response<bool> res = new();
        // Indicate success
        res.Data = true;
        try
        {
            // Get the response
            HttpResponseMessage response = await this._http.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            // Check the status code for the response
            if (response.IsSuccessStatusCode == true)
            {
                // Get the stream
                await response.Content.CopyToAsync(stream);
            }
            else
            {
                // Get string data
                string data = await response.Content.ReadAsStringAsync();
                // Add error data
                res.Data = false;
                res.Message = $"DownloadFile: {uri}. {Regex.Unescape(data)}";
            }
        }
        catch (Exception ex)
        {
            // Add exception data
            res.Data = false;
            res.Message = $"DownloadFile: {uri}. {ex.ToString()}";
        }
        // Return the response
        return res;
    }
    public void Exportar(string table, string type, Query query = null)
    {
        navigationManager.NavigateTo(query != null ? query.ToUrl($"{_http.BaseAddress}{table}/exportTo") : $"{_http.BaseAddress}{table}/exportTo", true);
    }

    public async Task<Response<T>> GetData<T>(string uri)
    {
        Response<T> res = new();
        try
        {
            res = await _http.GetFromJsonAsync<Response<T>>(uri);

        }
        catch (Exception e)
        {

            res.Message = e.Message;
        }

        return res;
    }

    public void ExportarToExcel(string url)
    {
        var uri = new Uri(url);

        navigationManager.NavigateTo(uri.AbsoluteUri, true);

    }

    public async Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
    {
        MailMessage message = new MailMessage();
        SmtpClient smtp = new SmtpClient();
        message.From = new MailAddress(_mailConfig.FromEmail);
        message.To.Add(new MailAddress(ToEmail));
        message.Subject = Subject;
        message.IsBodyHtml = true;
        message.Body = HTMLBody;
        smtp.Port = _mailConfig.Port;
        smtp.Host = _mailConfig.Host;
        smtp.TargetName = "STARTTLS/smtp.office365.com";
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_mailConfig.Username, _mailConfig.Password);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

        await smtp.SendMailAsync(message);
    }



}
