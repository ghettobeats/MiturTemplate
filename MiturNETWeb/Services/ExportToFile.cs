using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace MiturNetWeb.Services;
public class ExportToFile : ExportServices
{

    private readonly HttpClientOptions options;
    private readonly IJSRuntime _jSRuntime;
    public NavigationManager _navigationManager;
    [Obsolete]
    private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

    [Obsolete]
    public ExportToFile(
        Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment, 
        IJSRuntime jSRuntime, 
        NavigationManager navigationManager,
        IOptions<HttpClientOptions> options)
    {
        Environment = _environment;
        _jSRuntime = jSRuntime;
        _navigationManager = navigationManager;
        this.options = options.Value;
    }

    public async Task<bool> ExportarDatos(IQueryable<object> datos, string fileName, Query query = null)
    {
        return await ExportToExcel(ApplyQuery(datos, (IQueryCollection) query), $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
    }

    public async Task<FileStreamResult> ExportarToExcel(IQueryable<object> datos, string fileName, Query query = null)
    {
        return ToExcel(ApplyQuery(datos, (IQueryCollection)query), $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}");
    }

    public async Task<bool> ToFile(FileResult fileResult)
    {
        string wwwPath = this.Environment.WebRootPath;
        string contentPath = this.Environment.ContentRootPath;

        string exportedFiles = Path.Combine(this.Environment.WebRootPath, @"download\");
        if (!Directory.Exists(exportedFiles))
        {
            Directory.CreateDirectory(exportedFiles);
        }

        //string exportedFiles = @"C:\SegasaSGOExportedFiles\";

        //if (!Directory.Exists(exportedFiles))
        //{
        //    Directory.CreateDirectory(exportedFiles);
        //}

        var pathFileName = $"{exportedFiles}{fileResult.FileDownloadName}";
        var pathURL = $"{options.URL}download/{fileResult.FileDownloadName}";

        if (fileResult is FileStreamResult)
        {

            using (var fileStream = File.Create(pathFileName))
            {
                var fileStreamResult = (FileStreamResult)fileResult;
                fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
                fileStreamResult.FileStream.CopyTo(fileStream);
                fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            }
        }

        if (File.Exists(pathFileName))
        {
            //await _jSRuntime.InvokeVoidAsync("triggerFileDownload", fileResult.FileDownloadName, pathURL);

             _navigationManager.NavigateTo(pathURL, true);

            //_client.ExportarToExcel(pathURL);
            //NavigationManager.NavigateTo(pathURL);



            return true;
        }
        else
        {
            return false;
        }
    }

}
