using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiturNetShared.Model.Pivot;

public class LocalidadesCliente
{
    public int id { get; set; }
    public int idEstatus { get; set; }
    public int idTipoLocalidad { get; set; }
    public int idProvincia { get; set; }
    public int idZona { get; set; }
    public string idCliente { get; set; }
    public int Codigo { get; set; }
    public string Descripcion { get; set; }
    public string Ciudad { get; set; }
    public double? Longitud { get; set; }
    public double? Latitud { get; set; }
    public string Direccion { get; set; }
    public object Memo { get; set; }
    public IdTipoLocalidadNavigation idTipoLocalidadNavigation { get; set; }
    public IdProvinciaNavigation idProvinciaNavigation { get; set; }
    public IdZonaNavigation idZonaNavigation { get; set; }
    public IdClienteNavigation idClienteNavigation { get; set; }
    public List<LocalidadPuesto> LocalidadPuesto { get; set; }
}

public class IdClienteNavigation
{
    public string idCliente { get; set; }
    public string Nombre { get; set; }
    public int Inactive { get; set; }
}

public class IdProvinciaNavigation
{
    public int id { get; set; }
    public int idEstatus { get; set; }
    public int Codigo { get; set; }
    public string Nombre { get; set; }
    public string Capital { get; set; }
}

public class IdTipoLocalidadNavigation
{
    public int id { get; set; }
    public int idEstatus { get; set; }
    public string Descripcion { get; set; }
}

public class IdZonaNavigation
{
    public int id { get; set; }
    public int idEstatus { get; set; }
    public int idRegion { get; set; }
    public string Detalle { get; set; }
}
