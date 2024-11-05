namespace MiturNetShared.Model.Formulario;

public class FormReporteCantidad
{
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow.Date;
    public DateTime FechaHasta { get; set; } = DateTime.UtcNow.Date;
    public int? Zone { get; set; }
    public int? Regione { get; set; }

}