namespace MiturNetDomain.Entities.Operation;

public partial class Vehiculo : EntityBase
{
    public string Modelo { get; set; }

    public string Marca { get; set; }

    public string Placa { get; set; }

    public int? IdChofer { get; set; }

    public int? IdEstadoVehiculo { get; set; }

    public int? IdEstado { get; set; }
  
    public virtual Chofer IdChoferNavigation { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; }

    public virtual EstadoVehiculo IdEstadoVehiculoNavigation { get; set; }
}
