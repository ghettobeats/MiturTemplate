namespace MiturNetAPI.Controllers;

public class ExportToFileController : ExportController
{

    public ExportToFileController()
    {
        
    }

    [HttpPost("ExportToExcel")]
    public async Task<FileStreamResult> ExportToExcel(string fileName, string classname, dynamic datos)
    {
        //Type type = Type.GetType(classname);
        //object oClass = Activator.CreateInstance(type);

        //Response<IEnumerable<type>> response = new();

        ////response.Data = await datos;

        //return ToExcel(ApplyQuery(response.Data.ToList().AsQueryable(), Request.Query), $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}");
        return null;
    }
}
