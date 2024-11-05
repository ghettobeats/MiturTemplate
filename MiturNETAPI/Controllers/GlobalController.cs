namespace MiturNetAPI.Controllers;
public class ChoferController : BaseController<Chofer, ChoferDto>
{
    private readonly IServiceBase<Chofer> _service;
    private readonly IMapper _mapper;

    public ChoferController(IServiceBase<Chofer> service, IMapper mapper) : base(service, mapper)
    {
    }
}

public class EstadoController : BaseController<Estado, EstadoDto>
{
    private readonly IServiceBase<Estado> _service;
    private readonly IMapper _mapper;

    public EstadoController(IServiceBase<Estado> service, IMapper mapper) : base(service, mapper)
    {
    }
}

public class EstadoChoferController : BaseController<EstadoChofer, EstadoChoferDto>
{
    private readonly IServiceBase<EstadoChofer> _service;
    private readonly IMapper _mapper;

    public EstadoChoferController(IServiceBase<EstadoChofer> service, IMapper mapper) : base(service, mapper)
    {
    }
}

public class EstadoSolicitudController : BaseController<EstadoSolicitud, EstadoSolicitudDto>
{
    private readonly IServiceBase<EstadoSolicitud> _service;
    private readonly IMapper _mapper;

    public EstadoSolicitudController(IServiceBase<EstadoSolicitud> service, IMapper mapper) : base(service, mapper)
    {
    }
}

public class EstadoVehiculoController : BaseController<EstadoVehiculo, EstadoVehiculoDto>
{
    private readonly IServiceBase<EstadoVehiculo> _service;
    private readonly IMapper _mapper;

    public EstadoVehiculoController(IServiceBase<EstadoVehiculo> service, IMapper mapper) : base(service, mapper)
    {
    }
}

public class SolicitudController : BaseController<Solicitud, SolicitudDto>
{
    private readonly IServiceBase<Solicitud> _service;
    private readonly IMapper _mapper;

    public SolicitudController(IServiceBase<Solicitud> service, IMapper mapper) : base(service, mapper)
    {
    }
}

public class SolicitudDetalleController : BaseController<SolicitudDetalle, SolicitudDetalleDto>
{
    private readonly IServiceBase<SolicitudDetalle> _service;
    private readonly IMapper _mapper;

    public SolicitudDetalleController(IServiceBase<SolicitudDetalle> service, IMapper mapper) : base(service, mapper)
    {
    }
}

public class VehiculoController : BaseController<Vehiculo, VehiculoDto>
{
    private readonly IServiceBase<Vehiculo> _service;
    private readonly IMapper _mapper;

    public VehiculoController(IServiceBase<Vehiculo> service, IMapper mapper) : base(service, mapper)
    {
    }
}




/*
public class AlmacenController : BaseController<Almacen, getAlmacen>
{
    IServiceBase<Almacen> _services;
    IMapper _mapper;
    public AlmacenController(IServiceBase<Almacen> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindAll")]
    public async Task<IActionResult> FindAll()
    {
        Response<IEnumerable<getAlmacenInclude>> res = new();

        var data = await _services.GetAll()
            .Include(db => db.IdProvinciaNavigation)
            .ToListAsync();
        res.Data = _mapper.Map<IEnumerable<getAlmacenInclude>>(data);

        //res.Data = _mapper.Map<IEnumerable<getAlmacenInclude>>(_services.GetAll()
        //    .Include(db => db.IdProvinciaNavigation).AsAsyncEnumerable());

        return Ok(res);
    }
}
public class ArmeriaController : BaseController<Armeria, getArmeria>
{
    public ArmeriaController(IServiceBase<Armeria> services, IMapper mapper) : base(services, mapper)
    {

    }
}
public class ClienteLocalController : BaseController<ClienteLocal, getClienteLocal>
{
    public ClienteLocalController(IServiceBase<ClienteLocal> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class ClienteServicioController : BaseController<ClienteServicio, getClienteServicio>
{
    public ClienteServicioController(IServiceBase<ClienteServicio> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class DiaFeriadoController : BaseController<DiaFeriado, getDiaFeriado>
{
    public DiaFeriadoController(IServiceBase<DiaFeriado> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class EstatusController : BaseController<Estatus, getEstatus>
{
    public EstatusController(IServiceBase<Estatus> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class FlotaController : BaseController<Flota, getFlota>
{
    public FlotaController(IServiceBase<Flota> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class LocalidadController : BaseController<Localidad, getLocalidad>
{
    IServiceBase<Localidad> _services;
    IMapper _mapper;
    public LocalidadController(IServiceBase<Localidad> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getLocalidadInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadInclude>>(_services.GetAll()
            .Include(db => db.idTipoLocalidadNavigation)
            .Include(c => c.idProvinciaNavigation)
            .Include(d => d.idZonaNavigation)
            .Include(e => e.idClienteNavigation).Where(p => p.idClienteNavigation != null)
            .AsAsyncEnumerable());
        return Ok(res);
    }


    [HttpGet("FindAllByID/{cliente}")]
    public async Task<IActionResult> getAllByID(string cliente)
    {
        Response<IEnumerable<getLocalidadInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadInclude>>(_services.FindBy(db => db.idCliente.Equals(cliente))
            .AsAsyncEnumerable())
            .Where(p => p.idClienteNavigation != null);
        return Ok(res);
    }
}
public class LocalidadLocalizadorController : BaseController<LocalidadLocalizador, getLocalidadLocalizador>
{
    IServiceBase<LocalidadLocalizador> _services;
    IMapper _mapper;
    public LocalidadLocalizadorController(IServiceBase<LocalidadLocalizador> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindByLocalidad/{localidad}")]
    public async Task<IActionResult> getAll(int localidad)
    {
        Response<IEnumerable<getLocalidadLocalizadorInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadLocalizadorInclude>>(_services.FindBy(db => db.idLocalidad.Equals(localidad))
            .Include(c => c.idLocalizadorNavigation)
            .Include(d => d.idTipoContactoNavigation)
            .AsAsyncEnumerable());
        return Ok(res);
    }


}
public class LocalidadPuestoController : BaseController<LocalidadPuesto, getLocalidadPuesto>
{
    IServiceBase<LocalidadPuesto> _services;
    IMapper _mapper;

    public LocalidadPuestoController(IServiceBase<LocalidadPuesto> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpPut("SoloPuesto/{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] getLocalidadPuestoExtra Dto)
    {
        Response<getLocalidadPuestoExtra> res = new();
        if (_services.Exists(db => db.id.Equals(id)))
        {
            await _services.UpdateAsync(_mapper.Map<LocalidadPuesto>(Dto));
            res.Data = Dto;
            return Ok(res);
        }
        else { return BadRequest(ModelState); }
    }

    [HttpGet("LocalidadPuestoById/{id}")]
    public async Task<IActionResult> getLocalidadPuestoById(int id)
    {
        Response<getLocalidadPuestoExtra> res = new();
        res.Data = _mapper.Map<getLocalidadPuestoExtra>(_services.FindBy(db => db.id.Equals(id)).FirstOrDefault());
        return Ok(res);
    }


    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getLocalidadPuestoInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadPuestoInclude>>(_services.GetAll()
            .Include(db => db.idTipoPersonaNavigation)
            .Include(db => db.idTipoPuestoNavigation)
            .Include(db => db.LocalidadPuestoTurno)
                .ThenInclude(db => db.idTipoDiaNavigation)
            .Include(db => db.LocalidadPuestoTurno)
                .ThenInclude(db => db.idTipoTurnoNavigation)
            .Include(db => db.LocalidadPuestoHerramienta)
            .Include(db => db.LocalidadPuestoTurno)
                .ThenInclude(db => db.idPersonaNavigation)
            .AsAsyncEnumerable());

        return Ok(res);
    }


    [HttpGet("FindByLocalidad/{localidad}")]
    public async Task<IActionResult> getAll(int localidad)
    {
        Response<IEnumerable<getLocalidadPuesto>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadPuesto>>(_services.FindBy(db => db.idLocalidad.Equals(localidad)).AsAsyncEnumerable());
        return Ok(res);
    }
}

public class LocalidadPuestoTurnoController : BaseController<LocalidadPuestoTurno, getLocalidadPuestoTurno>
{
    IServiceBase<LocalidadPuestoTurno> _services;
    IMapper _mapper;

    public LocalidadPuestoTurnoController(IServiceBase<LocalidadPuestoTurno> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }


    [HttpGet("FindByPuesto/{puesto}")]
    public async Task<IActionResult> getAll(int puesto)
    {
        Response<IEnumerable<getLocalidadPuestoTurnoInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadPuestoTurnoInclude>>(_services.FindBy(db => db.idLocalidadPuesto.Equals(puesto))
            .AsAsyncEnumerable());
        return Ok(res);
    }


    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getLocalidadPuestoTurnoInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadPuestoTurnoInclude>>(_services.GetAll()
            .AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpGet("GetOSAsignado")]
    public async Task<IActionResult> GetOSAsignado(string empleado)
    {
        Response<IEnumerable<getLocalidadPuestoTurnoInclude>> res = new();

        if (_services.Exists(db=> db.idPersona.Equals(empleado)))
        {
            res.Data =  _mapper.Map<IEnumerable<getLocalidadPuestoTurnoInclude>>(await _services.GetAll().ToListAsync());
            res.Message = "Este Oficial tiene servicio asignado. Revise, por favor";
        } 

        return  Ok(res);
    }
}

public class LocalizadorController : BaseController<Localizador, getLocalizador>
{
    public LocalizadorController(IServiceBase<Localizador> services, IMapper mapper) : base(services, mapper)
    {
    }
}

public class PersonaClaseController : BaseController<PersonaClase, getPersonaClase>
{
    IServiceBase<PersonaClase> _services;
    IMapper _mapper;
    public PersonaClaseController(IServiceBase<PersonaClase> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<includePersonaClase>> res = new();
        res.Data = _mapper.Map<IEnumerable<includePersonaClase>>(_services.GetAll()
            .Include(db => db.idTipoNominaNavigation).AsAsyncEnumerable());
        return Ok(res);
    }
}
public class ProvinciaController : BaseController<Provincia, getProvincia>
{
    public ProvinciaController(IServiceBase<Provincia> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class RadioController : BaseController<Radio, getRadio>
{
    public RadioController(IServiceBase<Radio> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class RegionController : BaseController<Region, getRegion>
{
    public RegionController(IServiceBase<Region> services, IMapper mapper) : base(services, mapper)
    {
    }
}

public class TipoArmaController : BaseController<TipoArma, getTipoArma>
{
    public TipoArmaController(IServiceBase<TipoArma> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class TipoContactoController : BaseController<TipoContacto, getTipoContacto>
{
    public TipoContactoController(IServiceBase<TipoContacto> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class TipoDiaController : BaseController<TipoDia, getTipoDia>
{
    public TipoDiaController(IServiceBase<TipoDia> services, IMapper mapper) : base(services, mapper)
    {
    }
}

public class TipoHerramientaClasificacionController : BaseController<TipoHerramientaClasificacion, getTipoHerramientaClasificacion>
{
    public TipoHerramientaClasificacionController(IServiceBase<TipoHerramientaClasificacion> services, IMapper mapper) : base(services, mapper)
    {
    }
}


public class TipoHerramientaController : BaseController<TipoHerramienta, getTipoHerramienta>
{
    IServiceBase<TipoHerramienta> _services;
    IMapper _mapper;
    public TipoHerramientaController(IServiceBase<TipoHerramienta> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getTipoHerramientaInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getTipoHerramientaInclude>>(_services.GetAll()
            .Include(db => db.idTipoHerramientaClasificacionNavigation).AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpGet("Tipo/{Tipo}")]
    public async Task<IActionResult> getByTipo(int Tipo)
    {
        Response<IEnumerable<getTipoHerramienta>> res = new();
        res.Data = _mapper.Map<IEnumerable<getTipoHerramienta>>(_services.FindBy(db => db.idTipoHerramientaClasificacion
            .Equals(Tipo))
            .AsAsyncEnumerable());
        return Ok(res);
    }
}

public class TipoHerramientaDetalleController : BaseController<TipoHerramientaDetalle, getTipoHerramientaDetalle>
{
    IServiceBase<TipoHerramientaDetalle> _services;
    IMapper _mapper;
    public TipoHerramientaDetalleController(IServiceBase<TipoHerramientaDetalle> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getTipoHerramientaDetalleInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getTipoHerramientaDetalleInclude>>(_services.GetAll()
            .Include(db => db.idTipoHerramientaNavigation)
                 .ThenInclude(c => c.idTipoHerramientaClasificacionNavigation)
            .Include(z => z.idAlmacenNavigation).AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpGet("FindByCodigo/{codigo}")]
    public async Task<IActionResult> getAllByCodigo(string codigo)
    {
        Response<IEnumerable<getTipoHerramientaDetalle>> res = new();
        res.Data = _mapper.Map<IEnumerable<getTipoHerramientaDetalle>>(_services.FindBy(db => db.Codigo.Equals(codigo))
            .AsAsyncEnumerable());
        return Ok(res);
    }
}


public class TipoLocalidadController : BaseController<TipoLocalidad, getTipoLocalidad>
{
    public TipoLocalidadController(IServiceBase<TipoLocalidad> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class TipoNominaController : BaseController<TipoNomina, getTipoNomina>
{
    public TipoNominaController(IServiceBase<TipoNomina> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class TipoPuestoController : BaseController<TipoPuesto, getTipoPuesto>
{
    public TipoPuestoController(IServiceBase<TipoPuesto> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class TipoTurnoController : BaseController<TipoTurno, getTipoTurno>
{
    public TipoTurnoController(IServiceBase<TipoTurno> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class TipoUniformeController : BaseController<TipoUniforme, getTipoUniforme>
{
    public TipoUniformeController(IServiceBase<TipoUniforme> services, IMapper mapper) : base(services, mapper)
    {
    }
}

public class ZonaController : BaseController<Zona, getZona>
{
    IServiceBase<Zona> _services;
    IMapper _mapper;
    public ZonaController(IServiceBase<Zona> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getZonaInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getZonaInclude>>(_services.GetAll()
            .Include(db => db.idRegionNavigation).AsAsyncEnumerable());
        return Ok(res);
    }
}

public partial class ZonaPersonaController : BaseController<ZonaPersona, getZonaPersona>
{
    IServiceBase<ZonaPersona> _services;
    IMapper _mapper;
    public ZonaPersonaController(IServiceBase<ZonaPersona> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }
}
public class ZonaUsuarioController : BaseController<ZonaUsuario, getZonaUsuario>
{
    IServiceBase<ZonaUsuario> _services;
    IMapper _mapper;

    public ZonaUsuarioController(IServiceBase<ZonaUsuario> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindAll")]
    public async Task<IActionResult> getAll()
    {
        Response<IEnumerable<getZonaUsuarioInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getZonaUsuarioInclude>>(_services.GetAll()
            .Include(c => c.idZonaNavigation)
            .Include(d => d.idAspNetUsersProfileNavigation)
                .ThenInclude(e => e.IdNavigation)
            .AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpGet("UsuariosZona")]
    public async Task<IActionResult> getUsuarioZona()
    {
        Response<IEnumerable<getZonaUsuarioInclude>> res = new();
        res.Data = _mapper.Map<IEnumerable<getZonaUsuarioInclude>>(_services.GetAll()
            .Include(d => d.idAspNetUsersProfileNavigation)
            .AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpPost("addRange")]
    public async Task<IActionResult> PostRange([FromBody] IEnumerable<getZonaUsuario> Dto)
    {
        Response<IEnumerable<getZonaUsuario>> res = new();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _services.InsertRange(_mapper.Map<IEnumerable<ZonaUsuario>>(Dto));
        //res.Data = Dto;
        return Ok(res);
    }

}
public class LocalidadPuestoHerramientaController : BaseController<LocalidadPuestoHerramienta, getLocalidadPuestoHerramienta>
{
    IServiceBase<LocalidadPuestoHerramienta> _services;
    IMapper _mapper;
    public LocalidadPuestoHerramientaController(IServiceBase<LocalidadPuestoHerramienta> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("FindByPuesto/{puesto}")]
    public async Task<IActionResult> getAll(int puesto)
    {
        Response<IEnumerable<getLocalidadPuestoHerramienta>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadPuestoHerramienta>>(_services.FindBy(db => db.idLocalidadPuesto.Equals(puesto))
            .AsAsyncEnumerable());
        return Ok(res);
    }

    [HttpGet("FindByPuestoDetalle/{puesto}")]
    public async Task<IActionResult> getAllPuesto(int puesto)
    {
        Response<IEnumerable<getLocalidadPuestoHerramientaIncludeExtra>> res = new();
        res.Data = _mapper.Map<IEnumerable<getLocalidadPuestoHerramientaIncludeExtra>>(_services.FindBy(db => db.idLocalidadPuesto.Equals(puesto))
            .Include(c => c.idTipoHerramientaNavigation)
                .ThenInclude(b => b.idTipoHerramientaClasificacionNavigation)
            .Include(d => d.idTipoHerramientaDetalleNavigation)
            .AsAsyncEnumerable());
        return Ok(res);
    }

}
public class TmpNominaLoteController : BaseController<TmpNominaLote, getTmpNominaLote>
{
    public TmpNominaLoteController(IServiceBase<TmpNominaLote> services, IMapper mapper) : base(services, mapper)
    {
    }
}
public class TipoHerramientaMovimientoController : BaseController<TipoHerramientaMovimiento, getTipoHerramientaMovimiento>
{
    public TipoHerramientaMovimientoController(IServiceBase<TipoHerramientaMovimiento> services, IMapper mapper) : base(services, mapper)
    {
    }
}

public partial class PersonaController : BaseController<Persona, getPersona>
{
    IServiceBase<Persona> _services;
    IMapper _mapper;
    public PersonaController(IServiceBase<Persona> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }
}

public partial class ClienteController : BaseController<Cliente, getCliente>
{
    IServiceBase<Cliente> _services;
    IMapper _mapper;
    public ClienteController(IServiceBase<Cliente> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }
}

public partial class ClientesOperacionController : BaseController<ClienteOperacion, getOperacionOficial>
{
    public ClientesOperacionController(IServiceBase<ClienteOperacion> services, IMapper mapper) : base(services, mapper)
    {
    }
}

public partial class AprobacionUsuarioController : BaseController<AprobacionUsuario, getAprobacionUsuario>
{
    IServiceBase<AprobacionUsuario> _services;
    IMapper _mapper;
    public AprobacionUsuarioController(IServiceBase<AprobacionUsuario> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }
}

public partial class AprobacionController : BaseController<Aprobacion, getAprobacion>
{
    IServiceBase<Aprobacion> _services;
    IMapper _mapper;
    public AprobacionController(IServiceBase<Aprobacion> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }
}


public partial class AprobacionDetalleController : BaseController<AprobacionDetalle, getAprobacionDetalle>
{
    public AprobacionDetalleController(IServiceBase<AprobacionDetalle> services, IMapper mapper) : base(services, mapper)
    {

    }
}

public class AprobacionNotaController : BaseController<AprobacionNota, getAprobacionNota>
{
    public AprobacionNotaController(IServiceBase<AprobacionNota> services, IMapper mapper) : base(services, mapper)
    {
    }
}

//public class LocalidadesController : BaseController<Localidades, getLocalidades>
//{
//    public LocalidadesController(IServiceBase<Localidades> services, IMapper mapper) : base(services, mapper)
//    {
//    }
//}

*/

