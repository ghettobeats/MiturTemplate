namespace MiturNetShared.Model.Formulario;

public class FormMovimientoHerramienta
{
    public int? Tipo { get; set; }
    public int? Zone { get; set; }
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow.Date;
    public DateTime FechaHasta { get; set; } = DateTime.UtcNow.Date; 
    public string? Oficial { get; set; }
}