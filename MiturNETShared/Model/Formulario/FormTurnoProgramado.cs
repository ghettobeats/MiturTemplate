namespace MiturNetShared.Model.Formulario;

public class FormTurnoProgramado
{
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow.Date;
    public DateTime FechaHasta { get; set; } = DateTime.UtcNow.Date;
    public int? zona { get; set; }
    public bool Completed { get; set; } = false;
    public bool Processed { get; set; } = false;
    public int aprobacion { get; set; } = 0;
    public string? Clase { get; set; }
    public string? Oficial { get; set; }


}
