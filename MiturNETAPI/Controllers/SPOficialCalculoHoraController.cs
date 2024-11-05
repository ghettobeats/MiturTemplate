namespace MiturNetAPI.Controllers;

/*
public class SPOficialCalculoHoraController : ExportController
{
    private readonly IServiceNoEntity<SPOficialCalculoHora> _services;
    private readonly IMapper _mapper;
   public SPOficialCalculoHoraController(IServiceNoEntity<SPOficialCalculoHora> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("CalculoOficial")]
    public async Task<IActionResult> GetCalculoOficial(DateTime FechaDesde, DateTime FechaHasta)
    {
        Response<IAsyncEnumerable<SPOficialCalculoHora>> response = new();
        try
        {
            var _getVistaReporteOficial = await _services.ExcuteStoreProcedure("getOficialCalculoHora {0}, {1}", FechaDesde, FechaHasta);
            if (_getVistaReporteOficial is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getVistaReporteOficial;
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