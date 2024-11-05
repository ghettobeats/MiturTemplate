namespace MiturNetDomain.Entities.Operation;

public partial class Estado : EntityBase
{
   

    public string NombreEstado { get; set; }

    public string Descripcion { get; set; }
    

    public virtual ICollection<Chofer> Choferes { get; set; } = new List<Chofer>();

    public virtual ICollection<EstadoChofer> EstadosChofer { get; set; } = new List<EstadoChofer>();

    public virtual ICollection<EstadoSolicitud> EstadosSolicitudes { get; set; } = new List<EstadoSolicitud>();

    public virtual ICollection<EstadoVehiculo> EstadosVehiculos { get; set; } = new List<EstadoVehiculo>();

    public virtual ICollection<SolicitudDetalle> SolicitudDetalles { get; set; } = new List<SolicitudDetalle>();

    public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
