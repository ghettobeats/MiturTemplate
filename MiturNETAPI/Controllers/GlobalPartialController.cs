namespace MiturNetAPI.Controllers;

/*
public partial class ZonaPersonaController : BaseController<ZonaPersona, getZonaPersona>
{
    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getZonaPersonaInclude>> res = new();
        var data = await _services.GetAll().Include(db => db.idZonaNavigation).ToListAsync();
        res.Data = _mapper.Map<IEnumerable<getZonaPersonaInclude>>(data);

        return Ok(res);
    }
}
public partial class PersonaController : BaseController<Persona, getPersona>
{
    [HttpGet("Odoo/{id}")]
    public async Task<IActionResult> getById(int id)
    {
        Response<getPersona> res = new();
        res.Data = _mapper.Map<getPersona>(_services.FindBy(db => db.idExterno.Equals(id)).FirstOrDefault());
        return Ok(res);
    }

    [HttpPost("Odoo")]
    public async Task<IActionResult> Post([FromBody] getPersona Dto)
    {
        Response<getPersona> res = new();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _services.AddAsync(_mapper.Map<Persona>(Dto));
        res.Data = Dto;
        return Ok(res);
    }

    [HttpPut("Odoo/{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] getPersona Dto)
    {
        Response<getPersona> res = new();
        if (_services.Exists(db => db.idExterno.Equals(id)))
        {
            await _services.UpdateAsync(_mapper.Map<Persona>(Dto));
            res.Data = Dto;
            return Ok(res);
        }
        else { return BadRequest(ModelState); }
    }
}
public partial class ClienteController : BaseController<Cliente, getCliente>
{
    [HttpGet("Odoo/{id}")]
    public async Task<IActionResult> GetOdoobyId(int id)
    {
        Response<getCliente> res = new();
        var data = await _services.FindBy(db => db.idExterno.Equals(id)).FirstOrDefaultAsync();
        res.Data = _mapper.Map<getCliente>(data);

        if (string.IsNullOrWhiteSpace(res.Data.Codigo))
        {
            res.Message = "No existen datos a mostrar";
            res.Succes = false;
        }

        return Ok(res);
    }

    [HttpPost("Odoo")]
    public async Task<IActionResult> PostOdoo([FromBody] getCliente Dto)
    {
        Response<getCliente> res = new();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _services.AddAsync(_mapper.Map<Cliente>(Dto));
        res.Data = Dto;
        return Ok(res);
    }

    [HttpPut("Odoo/{id}")]
    public async Task<IActionResult> PutOdoo(int id, [FromBody] getCliente Dto)
    {
        Response<getCliente> res = new();
        if (_services.Exists(db => db.idExterno.Equals(id)))
        {
            await _services.UpdateAsync(_mapper.Map<Cliente>(Dto));
            res.Data = Dto;
            return Ok(res);
        }
        else { return BadRequest(ModelState); }
    }
}
public partial class AprobacionUsuarioController : BaseController<AprobacionUsuario, getAprobacionUsuario>
{
    [HttpGet("FindAll")]
    public async Task<IActionResult> FindAll()
    {
        Response<IEnumerable<getAprobacionUsuarioInclude>> res = new();
        var data = await _services.GetAll()
                .Include(db => db.idVistaUsuarioNavigation)
                .ToListAsync();
        res.Data = _mapper.Map<IEnumerable<getAprobacionUsuarioInclude>>(data);

        return Ok(res);
    }

    [HttpGet("Flujo/{usuario}")]
    public async Task<IActionResult> Flujo(string usuario)
    {
        int flujo;
        Response<int> res = new();


        bool isBoolFlujo = _services.Exists(x => x.idUsuario.Equals(usuario) && x.idEstatus.Equals(1));
        if (isBoolFlujo)
        {
            flujo = _services.FindBy(db => db.idUsuario.Equals(usuario)).FirstOrDefault().Orden;
        }
        else
        {
            flujo = 0;
        }



        res.Data = _mapper.Map<int>(flujo);

        return Ok(res);
    }

    [HttpGet("EnviarOdoo")]
    public async Task<IActionResult> FlujoEnviarOdoo()
    {
        int flujo;
        Response<int> res = new();
        flujo = _services.GetAll().ToListAsync().Result.Max(d => d.Orden);        

        res.Data = _mapper.Map<int>(flujo);

        return Ok(res);
    }

    

}
public partial class AprobacionController : BaseController<Aprobacion, getAprobacion>
{
    [HttpGet("FindAll")]
    public async Task<IActionResult> FindAll()
    {
        Response<IEnumerable<getAprobacionInclude>> res = new();
        var data = await _services.GetAll().ToListAsync();
        res.Data = _mapper.Map<IEnumerable<getAprobacionInclude>>(data);

        return Ok(res);
    }
}
public class AprobacionValidarFlujoController : ExportController
{
    private readonly IServiceNoEntity<SPValidarFlujo> _services;
    private readonly IMapper _mapper;

    public AprobacionValidarFlujoController(IServiceNoEntity<SPValidarFlujo> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("ValidarFlujo/{usuario}")]
    public async Task<IActionResult> GetValidarFlujoAprobacion(string usuario)
    {
        Response<IEnumerable<SPValidarFlujo>> response = new();
        try
        {
            var _getFljuo = await _services.ExecWithStoreProcedure("validarFlujoAprobacion {0}", usuario);
            if (_getFljuo.Count() <= 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getFljuo;
            }
        }
        catch (Exception e)
        {
            response.Succes = false;
            response.Message = e.Message;
        }

        return Ok(response);
    }
}

*/