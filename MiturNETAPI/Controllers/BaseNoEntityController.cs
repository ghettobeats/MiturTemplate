namespace MiturNetAPI.Controllers;

public class BaseNoEntityController<T, Y> : ExportController where T : class where Y : class
{
    private readonly IServiceNoEntity<T> _services;
    private readonly IMapper _mapper;
    public BaseNoEntityController(IServiceNoEntity<T> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("ExportTo")]
    public FileStreamResult ExportToExcel()
    {
        Response<IEnumerable<Y>> res = new();
        res.Data = _mapper.Map<IEnumerable<Y>>(_services.GetAll().AsAsyncEnumerable());
        return ToExcel(ApplyQuery(res.Data.ToList().AsQueryable(), Request.Query), $"{typeof(T).Name}_{DateTime.Now:yyyyMMddHHmmss}");
    }
}
