namespace MiturNetAPI.Controllers;
/*
public class SPZonaActivaController : ExportController
{
    private readonly IServiceNoEntity<SPZonaActiva> _services;
    private readonly IMapper _mapper;
   
    public SPZonaActivaController(IServiceNoEntity<SPZonaActiva> services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("ZonaActivaPorDia")]
    public async Task<IActionResult> GetZonaActivaPorDia(DateTime FechaDesde, DateTime FechaHasta, int Tipo, string? Usuario)
    {
        Response<IAsyncEnumerable<SPZonaActiva>> response = new();
        try
        {
            var _getZonaActiva = await _services.ExcuteStoreProcedure("getZonaActiva {0}, {1}, {2}, {3}", FechaDesde, FechaHasta, Tipo, Usuario);
            if (_getZonaActiva is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getZonaActiva;
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
