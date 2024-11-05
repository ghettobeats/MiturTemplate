namespace MiturNetShared.Model.Formulario;

public class FormConsolidadoNomina
{
    public DateTime FechaDesde { get; set; } = DateTime.Now;
    public DateTime FechaHasta { get; set; } = DateTime.Now;
    public int? Nomina { get; set; }
}
