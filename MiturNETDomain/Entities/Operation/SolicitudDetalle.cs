namespace MiturNetDomain.Entities.Operation;
public partial class SolicitudDetalle : EntityBase
{
   
    public int? IdSolicitud { get; set; }

    public int? IdPasajero { get; set; }

    public int? IdChofer { get; set; }

    public int? IdEstado { get; set; }

    public int? IdVehiculo { get; set; }  

    public virtual Chofer IdChoferNavigation { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; }

    public virtual Solicitud IdSolicitudNavigation { get; set; }
}
