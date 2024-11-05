namespace MiturNetAPI.Controllers;

/*
public class VistaPersonaController : BaseNoEntityController<VistaPersona, getvistaPersona>
{
    IServiceNoEntity<VistaPersona> _services;
    IMapper _mapper;

    public VistaPersonaController(IServiceNoEntity<VistaPersona> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Response<IEnumerable<getvistaPersona>> res = new();
        res.Data = _mapper.Map<IEnumerable<getvistaPersona>>(_services.GetAll().AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        Response<getvistaPersona> res = new();
        res.Data = _mapper.Map<getvistaPersona>(_services.FindBy(db => db.idPersona.Equals(id)).FirstOrDefault());
        return Ok(res);
    }

    [HttpGet("getPersonaByName/{descripcion}")]
    public async Task<IActionResult> vistaPersonaByName(string descripcion)
    {
        Response<IEnumerable<getvistaPersona>> res = new();
        res.Data = _mapper.Map<IEnumerable<getvistaPersona>>(_services.FindBy(db => db.Nombre.StartsWith(descripcion)));
        return Ok(res);
    }

    [HttpGet("getOficialesActivos")]
    public async Task<IActionResult> Activos()
    {
        Response<IEnumerable<getvistaPersona>> res = new();
        res.Data = _mapper.Map<IEnumerable<getvistaPersona>>(_services.FindBy(db => db.Inactive == 0));
        return Ok(res);
    }
}
public class VistaTipoPersonaController : BaseNoEntityController<VistaTipoPersona, getvistaTipoPersona>
{
    IServiceNoEntity<VistaTipoPersona> _services;
    IMapper _mapper;

    public VistaTipoPersonaController(IServiceNoEntity<VistaTipoPersona> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Response<IEnumerable<getvistaTipoPersona>> res = new();
        res.Data = _mapper.Map<IEnumerable<getvistaTipoPersona>>(_services.GetAll().AsAsyncEnumerable());
        return Ok(res);
    }
}
public class VistaOSPendienteController : BaseNoEntityController<VistaOSPendiente, getVistaOSPendiente>
{
    IServiceNoEntity<VistaOSPendiente> _services;
    IMapper _mapper;

    public VistaOSPendienteController(IServiceNoEntity<VistaOSPendiente> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Response<IEnumerable<getVistaOSPendiente>> res = new();
        res.Data = _mapper.Map<IEnumerable<getVistaOSPendiente>>(_services.GetAll().AsAsyncEnumerable());
        return Ok(res);
    }
}
public class AspNetUsersProfileController : BaseNoEntityController<AspNetUsersProfile, getAspNetUsersProfile>
{
    IServiceNoEntity<AspNetUsersProfile> _services;
    IMapper _mapper;

    public AspNetUsersProfileController(IServiceNoEntity<AspNetUsersProfile> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Response<IEnumerable<getAspNetUsersProfile>> res = new();
        res.Data = _mapper.Map<IEnumerable<getAspNetUsersProfile>>(_services.GetAll()
            .Include(db => db.IdNavigation)
            .Include(db => db.ZonaUsuario)
                .ThenInclude(db => db.idZonaNavigation)
                    .ThenInclude(db => db.idRegionNavigation)
            .AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpGet("getUsuarios")]
    public async Task<IActionResult> GetAllUsers()
    {
        Response<IEnumerable<getUsuarios>> res = new();
        res.Data = _mapper.Map<IEnumerable<getUsuarios>>(_services.GetAll()
            .Include(db => db.IdNavigation)
            .AsAsyncEnumerable());
        return Ok(res);
    }
}
public class NoficacionClienteController : BaseNoEntityController<NotificacionCliente, getNotificacionCliente>
{
    IServiceNoEntity<NotificacionCliente> _services;
    IMapper _mapper;

    public NoficacionClienteController(IServiceNoEntity<NotificacionCliente> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Response<IEnumerable<getNotificacionCliente>> res = new();
        res.Data = _mapper.Map<IEnumerable<getNotificacionCliente>>(_services.GetAll().AsAsyncEnumerable());
        return Ok(res);
    }
    [HttpGet("notificacion")]
    public async Task<IActionResult> Get(int puesto, int turno)
    {
        Response<getNotificacionCliente> res = new();
        res.Data = _mapper.Map<getNotificacionCliente>(_services.FindBy(
            db => db.idLocalidadPuesto.Equals(puesto) && db.idTipoTurno.Equals(turno)
            ).FirstOrDefault());
        if (res.Data is null)
        {
            res.Message = "No existen datos a mostrar";
            res.Succes = false;
        }
        return Ok(res);
    }

}
*/