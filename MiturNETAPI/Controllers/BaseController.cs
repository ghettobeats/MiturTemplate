namespace MiturNetAPI.Controllers;
public class BaseController<T, Y> : ExportController where T : EntityBase where Y : class
{
    private readonly IServiceBase<T> _services;
    private readonly IMapper _mapper;
    public BaseController(IServiceBase<T> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> getAll()
    {        
        Response<IEnumerable<Y>> res = new();
        var data = await _services.GetAll().ToListAsync();
        res.Data = _mapper.Map<IEnumerable<Y>>(data);       
        res.Message = res.Data.Count() <= 0 ? "No existen datos a mostrar": "DONE";       
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Response<Y> res = new();
        var data = await _services.FindBy(db => db.Id.Equals(id)).FirstOrDefaultAsync();         
        //res.Data = _mapper.Map<Y>(_services.FindBy(db => db.id.Equals(id)).FirstOrDefault());
        res.Data = _mapper.Map<Y>(data);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Y Dto, CancellationToken cancellationToken = default)
    {
        Response<Y> res = new();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _services.AddAsync(_mapper.Map<T>(Dto));
        res.Data = Dto;

        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Y Dto)
    {
        Response<Y> res = new();
        if (_services.Exists(db => db.Id.Equals(id)))
        {
            var entityToUpdate = _mapper.Map<T>(Dto);
            entityToUpdate.Id = id;
            await _services.UpdateAsync(entityToUpdate);
            res.Data = Dto;
            return Ok(res);
        }
        else { return BadRequest(ModelState); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Response<Y> res = new();
        if (_services.Exists(db => db.Id.Equals(id)))
        {
            var _delete = await _services.FindBy(db => db.Id.Equals(id)).FirstOrDefaultAsync();
            _services.Delete(_delete);
            res.Data = _mapper.Map<Y>(_delete); ;
            return Ok(res);
        }
        else { return NotFound("Registro no existe"); }
    }

    [HttpGet("ExportTo")]
    public FileStreamResult ExportToExcel()
    {
        Response<IEnumerable<Y>> res = new();
        res.Data = _mapper.Map<IEnumerable<Y>>(_services.GetAll().AsAsyncEnumerable());
        return ToExcel(ApplyQuery(res.Data.ToList().AsQueryable(), Request.Query), $"{typeof(T).Name}_{DateTime.Now:yyyyMMddHHmmss}");
    }

}

