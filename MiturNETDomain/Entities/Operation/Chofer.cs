namespace MiturNetDomain.Entities.Operation;

public partial class Chofer : EntityBase
{
    public string IdEmpleado { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Cedula { get; set; }

    public string Email { get; set; }

    public string Telefono { get; set; }

    public int? IdEstadoChofer { get; set; }

    public int? IdEstado { get; set; }

    public bool? EsConductor { get; set; }
     
    public virtual EstadoChofer IdEstadoChoferNavigation { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; }

    public virtual ICollection<SolicitudDetalle> SolicitudDetalles { get; set; } = new List<SolicitudDetalle>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
