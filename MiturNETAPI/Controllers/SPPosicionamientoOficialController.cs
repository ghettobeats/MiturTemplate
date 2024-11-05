namespace MiturNetAPI.Controllers;

/*
public class SPPosicionamientoOficialController : ExportController
{
    private readonly IServiceNoEntity<SPPosicionamientoOficial> _services;
    private readonly IMapper _mapper;
       public SPPosicionamientoOficialController(IServiceNoEntity<SPPosicionamientoOficial> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;       
    }

    [HttpGet("PosicionaOficial")]
    public async Task<IActionResult> GetPosicionaOficial(bool Completed, int? LaZona, DateTime FechaDesde, DateTime FechaHasta)
    {
        Response<IAsyncEnumerable<SPPosicionamientoOficial>> response = new();
        try
        {
            var _getVistaPosicionamientoOficial = await _services.ExcuteStoreProcedure("getPosicionamientoOficial {0}, {1}, {2}, {3}", Completed, LaZona, FechaDesde, FechaHasta);
            if (_getVistaPosicionamientoOficial is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getVistaPosicionamientoOficial;
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

public class SPClienteOperacionGeneralRPTController : ExportController
{
    private readonly IServiceNoEntity<SPClienteOperacionGeneralRPT> _services;
    private readonly IMapper _mapper;
    public SPClienteOperacionGeneralRPTController(IServiceNoEntity<SPClienteOperacionGeneralRPT> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("ReporteOSCantidad")]
    public async Task<IActionResult> GetPosicionaOficial(DateTime FechaDesde, DateTime FechaHasta, int? Zone, int? Regione)
    {
        Response<IAsyncEnumerable<SPClienteOperacionGeneralRPT>> response = new();
        try
        {
            var _getVistaPosicionamientoOficial = await _services.ExcuteStoreProcedure("getClienteOperacionGeneralRPT {0}, {1}, {2}, {3}", FechaDesde, FechaHasta, Zone, Regione);
            if (_getVistaPosicionamientoOficial is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getVistaPosicionamientoOficial;
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
