namespace MiturNetAPI.Controllers;
/*
public class ClienteOperacionController : BaseNoEntityController<ScalarValue, getScalarValue>
{
    IServiceNoEntity<ScalarValue> _services;
    IMapper _mapper;
    public ClienteOperacionController(IServiceNoEntity<ScalarValue> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("generaroperacion")]
    public async Task<IActionResult> PostClienteOperacion(DateTime Fecha, string Usuario)
    {
        Response<IAsyncEnumerable<ScalarValue>> response = new();
        try
        {
            var _getClienteOperacion = await _services.ExcuteStoreProcedure("insClienteOperacion {0}, {1}", Fecha, Usuario);
            if (_getClienteOperacion is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getClienteOperacion;
            }
        }
        catch (Exception e)
        {
            response.Succes = false;
            response.Message = e.Message;
        }

        return Ok(response);
    }

    [HttpGet("cierreoperacion")]
    public async Task<IActionResult> postCierreOperacion(DateTime Fecha, int Zone, bool completed, string Usuario)
    {
        Response<IAsyncEnumerable<ScalarValue>> response = new();
        try
        {
            var _getCierreClienteOperacion = await _services.ExcuteStoreProcedure("putClienteOperacionIsCompleted {0}, {1}, {2}, {3}", Fecha, completed, Zone, Usuario);
            if (_getCierreClienteOperacion is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getCierreClienteOperacion;
            }
        }
        catch (Exception e)
        {
            response.Succes = false;
            response.Message = e.Message;
        }

        return Ok(response);
    }


    [HttpGet("cambiaroficial")]
    public async Task<IActionResult> PostClienteOperacionPersona(int ClienteOperacionID, string PersonaID, string Memo, int EstatusID, string Usuario, DateTime Fecha)
    {
        Response<IAsyncEnumerable<ScalarValue>> response = new();
        try
        {
            var _getCambiarOficial = await _services.ExcuteStoreProcedure("insClienteOperacionByPersona {0}, {1}, {2}, {3}, {4}, {5}", ClienteOperacionID, PersonaID, Memo, EstatusID, Usuario, Fecha);
            if (_getCambiarOficial is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getCambiarOficial;
            }
        }
        catch (Exception e)
        {
            response.Succes = false;
            response.Message = e.Message;
        }

        return Ok(response);
    }

    [HttpGet("LocalidadHoraContrada")]
    public async Task<IActionResult> GETLocalidadHoraContrada(int idLocalidad, Decimal Horas)
    {
        Response<IAsyncEnumerable<ScalarValue>> response = new();
        try
        {
            var _getLocalidadHoraContrada = await _services.ExcuteStoreProcedure("putLocalidadHoraContrada {0}, {1}", idLocalidad, Horas);
            if (_getLocalidadHoraContrada is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getLocalidadHoraContrada;
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

public class VistaCalculoNominaController : BaseNoEntityController<VistaCalculoNomina, VistaCalculoNomina>
{
    IServiceNoEntity<VistaCalculoNomina> _services;
    IMapper _mapper;
    public VistaCalculoNominaController(IServiceNoEntity<VistaCalculoNomina> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCalculoNomina(DateTime Fecha, string Usuario)
    {
        var _getClienteOperacion = await _services.ExcuteStoreProcedure("insClienteOperacion {0}, {1}", Fecha, Usuario);

        return Ok(_getClienteOperacion);

    }
}

public class VistaFechaAbiertaOperacionController : BaseNoEntityController<VistaFechaAbiertaOperacion, VistaFechaAbiertaOperacion>
{
    IServiceNoEntity<VistaFechaAbiertaOperacion> _services;
    IMapper _mapper;
    public VistaFechaAbiertaOperacionController(IServiceNoEntity<VistaFechaAbiertaOperacion> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    //[HttpGet("getFechaDisponible")]
    //public async Task<IActionResult> vistaFechaDisponible()
    //{
    //    Response<IEnumerable<getFechaDiaOperacion>> response = new();
    //    try
    //    {
    //        var _getFechaDiaOperacion = _services.GetAll();
    //        if (_getFechaDiaOperacion.Count() < 1)
    //        {
    //            response.Succes = false;
    //            response.Message = "No existen datos a mostrar";
    //        }
    //        else
    //        {
    //            response.Data = _mapper.Map<IEnumerable<getFechaDiaOperacion>>(_getFechaDiaOperacion);
    //        }
    //    }
    //    catch (Exception e)
    //    {

    //        response.Succes = false;
    //        response.Message = e.Message;
    //    }

    //    return Ok(response);
    //}


    [HttpGet]
    public async Task<IActionResult> getFechaAbiertaOperacion(string Usuario)
    {
        Response<IAsyncEnumerable<VistaFechaAbiertaOperacion>> response = new();
        try
        {
            var _getFechaAbiertaOperacion = await _services.ExcuteStoreProcedure("getFechaAbiertaOperacion {0}", Usuario);
            if (_getFechaAbiertaOperacion is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getFechaAbiertaOperacion;
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

public class SPClienteOperacionController : BaseNoEntityController<SPClienteOperacion, SPClienteOperacion>
{
    IServiceNoEntity<SPClienteOperacion> _services;
    IMapper _mapper;
    public SPClienteOperacionController(IServiceNoEntity<SPClienteOperacion> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> getClienteOperacion(bool Completed, DateTime Fecha, int? Zone, string Usuario)
    {
        Response<IAsyncEnumerable<SPClienteOperacion>> response = new();
        try
        {
            var _getSPClienteOperacion = await _services.ExcuteStoreProcedure("getClienteOperacion {0}, {1}, {2}, {3}", Completed, Zone, Fecha, Usuario);
            if (_getSPClienteOperacion is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getSPClienteOperacion;
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

public class SPClienteOperacionGeneralController : BaseNoEntityController<SPClienteOperacionGeneral, SPClienteOperacionGeneral>
{
    IServiceNoEntity<SPClienteOperacionGeneral> _services;
    IMapper _mapper;
    public SPClienteOperacionGeneralController(IServiceNoEntity<SPClienteOperacionGeneral> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> getClienteOperacionGeneral(DateTime fechaDesde, DateTime fechaHasta, bool Completed, int? zona, bool Processed, int aprobacion, string Usuario)
    {
        Response<IAsyncEnumerable<SPClienteOperacionGeneral>> response = new();
        try
        {
            var _getSPClienteOperacion = await _services.ExcuteStoreProcedure("getClienteOperacionGeneral {0}, {1}, {2}, {3}, {4}, {5}, {6}", fechaDesde, fechaHasta, Completed, zona, Processed, aprobacion, Usuario);
            if (_getSPClienteOperacion is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getSPClienteOperacion;
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

public class SPClienteOperacionOficialesController : BaseNoEntityController<SPClienteOperacion, SPClienteOperacion>
{
    IServiceNoEntity<SPClienteOperacion> _services;
    IMapper _mapper;
    public SPClienteOperacionOficialesController(IServiceNoEntity<SPClienteOperacion> services, IMapper mapper) : base(services, mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> getClienteOperacion(DateTime fechaDesde, DateTime fechaHasta, bool completed, bool processed, string? clase, string? oficial)
    {
        Response<IAsyncEnumerable<SPClienteOperacion>> response = new();
        try
        {
            var _getSPClienteOperacion = await _services.ExcuteStoreProcedure("getClienteOperacionOficiales {0}, {1}, {2}, {3}, {4}, {5}", fechaDesde, fechaHasta, completed, processed, clase, oficial);
            if (_getSPClienteOperacion is null)
            {
                response.Succes = false;
                response.Message = "No existen datos a mostrar";
            }
            else
            {
                response.Data = _getSPClienteOperacion;
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