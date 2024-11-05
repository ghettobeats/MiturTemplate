namespace MiturNetDomain.Entities.Operation;
public partial class EstadoSolicitud : EntityBase
{

    public string Nombre { get; set; }

    public int? IdEstado { get; set; }  

    public virtual Estado IdEstadoNavigation { get; set; }

    public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
}
