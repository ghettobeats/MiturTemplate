namespace MiturNetDomain.Entities.Operation;

public partial class EstadoChofer : EntityBase
{
   

    public string Nombre { get; set; }

    public int? IdEstado { get; set; }   

    public virtual ICollection<Chofer> Choferes { get; set; } = new List<Chofer>();

    public virtual Estado IdEstadoNavigation { get; set; }
}
