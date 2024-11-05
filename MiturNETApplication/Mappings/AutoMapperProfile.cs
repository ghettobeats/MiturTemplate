namespace MiturNetApplication.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<Chofer, ChoferDto>().ReverseMap();
        CreateMap<Estado, EstadoDto>().ReverseMap();
        CreateMap<EstadoChofer, EstadoChoferDto>().ReverseMap();
        CreateMap<EstadoSolicitud, EstadoSolicitudDto>().ReverseMap();
        CreateMap<EstadoVehiculo, EstadoVehiculoDto>().ReverseMap();
        CreateMap<Solicitud, SolicitudDto>().ReverseMap();
        CreateMap<SolicitudDetalle, SolicitudDetalleDto>().ReverseMap();
        CreateMap<Vehiculo, VehiculoDto>().ReverseMap();
    }
}
