namespace MiturNetAPI.Controllers;
   /*
public class SPConsolidadoNominaController : ExportController
{
 
    private readonly IServiceNoEntity<SPConsolidadoNomina> _services;
    private readonly IMapper _mapper;
    public SPConsolidadoNominaController(IServiceNoEntity<SPConsolidadoNomina> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("ConsolidadoNomina")]
    public async Task<IActionResult> getCalculoNominaConsolidado(DateTime FechaDesde, DateTime FechaHasta, int Nomina)
    {
        Response<IEnumerable<SPConsolidadoNomina>> response = new();
        try
        {
            response.Data = await _services.ExecWithStoreProcedure("getConsolidadoNominaOS {0}, {1}, {2}", FechaDesde, FechaHasta, Nomina);
            if (response.Data.Count() <= 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar para las condiciones ingresadas.";
            }
        }
        catch (Exception e)
        {
            response.Succes = false;
            response.Message = e.Message;
        }

        return Ok(response);
    }

    [HttpGet("NominaProcesada")]
    public async Task<IActionResult> getNominaProcesada(DateTime FechaDesde, DateTime FechaHasta, int Nomina)
    {
        Response<IEnumerable<SPConsolidadoNomina>> response = new();
        try
        {
            response.Data = await _services.ExecWithStoreProcedure("getNominaProcesada {0}, {1}, {2}", FechaDesde, FechaHasta, Nomina);
            if (response.Data.Count() <= 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar para las condiciones ingresadas.";
            }
        }
        catch (Exception e)
        {
            response.Succes = false;
            response.Message = e.Message;
        }

        return Ok(response);
    }


    [HttpGet("ConsolidadoNominaFljuo")]
    public async Task<IActionResult> getCalculoNominaConsolidadoFlujo(int flujo, int? Nomina)
    {
        Response<IEnumerable<SPConsolidadoNomina>> response = new();
        try
        {
            response.Data = await _services.ExecWithStoreProcedure("getConsolidadoNominaOSFlujo {0}, {1}", flujo, Nomina);
            if (response.Data.Count() <= 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar para las condiciones ingresadas.";
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
public class ProcesarNominaController : ExportController
{
    private readonly IServiceNoEntity<ScalarValue> _services;
    private readonly IMapper _mapper;

    public ProcesarNominaController(IServiceNoEntity<ScalarValue> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("ProcesarNomina")]
    public async Task<IActionResult> postProcesarNomina(int Nomina, DateTime FechaDesde, DateTime FechaHasta, string Usuario)
    {
        Response<ScalarValue> response = new();
        try
        {
            var _getProcesarNomina = await _services.ExecProcedureNoReturn("[dbo].[InsNominaOS] {0}, {1}, {2}, {3}", Nomina, FechaDesde, FechaHasta, Usuario);
            if (_getProcesarNomina == 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = null;
                response.Message = _getProcesarNomina.ToString();
            }
        }
        catch (Exception e)
        {
            response.Succes = false;
            response.Message = e.Message;
        }

        return Ok(response);
    }

    [HttpGet("ReversarNomina")]
    public async Task<IActionResult> postReversarNomina(int Nomina, DateTime FechaDesde, DateTime FechaHasta, string Usuario)
    {
        Response<ScalarValue> response = new();
        try
        {
            var _getReversarNomina = await _services.ExecProcedureNoReturn("[dbo].[putNominaOS] {0}, {1}, {2}, {3}", Nomina, FechaDesde, FechaHasta, Usuario);
            if (_getReversarNomina == 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = null;
                response.Message = _getReversarNomina.ToString();
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
public class NominaGPController : ExportController
{
    private readonly IServiceNoEntity<NominaGP> _services;
    private readonly IMapper _mapper;

    public NominaGPController(IServiceNoEntity<NominaGP> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("NominaGP")]
    public async Task<IActionResult> getNominaGP(DateTime FechaDesde, DateTime FechaHasta, int Nomina)
    {
        Response<IEnumerable<NominaGP>> response = new();
        try
        {
            var _getNominaGP = await _services.ExecWithStoreProcedure("getNominaOSToGP {0}, {1}, {2}", FechaDesde, FechaHasta, Nomina);
            if (_getNominaGP.Count() <= 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getNominaGP;
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
public class SPNominaCodigoPagoController : ExportController
{
    private readonly IServiceNoEntity<SPNominaCodigoPago> _services;
    private readonly IMapper _mapper;

    public SPNominaCodigoPagoController(IServiceNoEntity<SPNominaCodigoPago> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("NominaCodigoPago")]
    public async Task<IActionResult> getNominaCodigoPago(int Nomina)
    {
        Response<IEnumerable<SPNominaCodigoPago>> response = new();
        try
        {
            var _getNominaCodigoPago = await _services.ExecWithStoreProcedure("NominaCodigoPago {0}", Nomina);
            if (_getNominaCodigoPago.Count() <= 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getNominaCodigoPago;
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
public class SPMovimientoHerramientaController : ExportController
{
    private readonly IServiceNoEntity<SPMovimientoHerramienta> _services;
    private readonly IMapper _mapper;

    public SPMovimientoHerramientaController(IServiceNoEntity<SPMovimientoHerramienta> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("MovimientoHerramienta")]
    public async Task<IActionResult> GetToolTracking(int? Tipo, int? Zone, DateTime FechaDesde, DateTime FechaHasta, string? usuario)
    {
        Response<IEnumerable<SPMovimientoHerramienta>> response = new();
        try
        {
            var _getSPMovimientoHerramienta = await _services.ExecWithStoreProcedure("getMovimientoHerramienta {0}, {1}, {2}, {3}, {4}", Tipo, Zone, FechaDesde, FechaHasta, usuario);
            if (_getSPMovimientoHerramienta.Count() <= 0)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getSPMovimientoHerramienta;
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
