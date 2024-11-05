using Microsoft.AspNetCore.Mvc;
using Radzen;

namespace MiturNetShared.Interface;
public interface IBaseHttpClient
{
    void SetupToken(string token);
    Task<Response<T>> Add<T>(object root, string uri);
    Task<Response<T>> Update<T>(object root, string uri);
    Task<Response<T>> Get<T>(string uri);
    Task<Response<T>> GetData<T>(string uri);
    Task<Response<bool>> Delete(string uri);
    Task<Response<T>> UploadFile<T>(Stream stream, string file_name, string uri);
    Task<Response<bool>> DownloadFile(Stream stream, string uri);
    void Exportar(string table, string type, Query query = null);
    void ExportarToExcel(string url);
    Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
}


