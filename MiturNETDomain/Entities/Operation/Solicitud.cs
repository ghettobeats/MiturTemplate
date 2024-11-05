namespace MiturNetDomain.Entities.Operation;

public partial class Solicitud : EntityBase
{
    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string Destino { get; set; }

    public int? CantidadPasajeros { get; set; }

    public byte[] Adjunto { get; set; }

    public string NumeroDocumento { get; set; }

    public string Proposito { get; set; }

    public string Comentario { get; set; }

    public int? IdEstadoSolicitud { get; set; }

    public int? IdEstado { get; set; }   

    public virtual Estado IdEstadoNavigation { get; set; }

    public virtual EstadoSolicitud IdEstadoSolicitudNavigation { get; set; }

    public virtual ICollection<SolicitudDetalle> SolicitudDetalles { get; set; } = new List<SolicitudDetalle>();
}
