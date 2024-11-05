namespace MiturNetDomain.Entities.Operation;

public partial class EstadoVehiculo : EntityBase
{
    

    public string Nombre { get; set; }

    public int? IdEstado { get; set; }
   
    public virtual Estado IdEstadoNavigation { get; set; }

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
