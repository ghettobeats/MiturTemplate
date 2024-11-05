namespace MiturNetApplication.Dtos.AccountViewModels;
public class RegisterViewModel
{
    [Required]
    [Display(Name = "Nombre")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Correo Electrónico")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "El {0} debe de ser por lo menos {2} y como máximo {1} carácteres de longitud.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Contraseña")]
    [Compare("Password", ErrorMessage = "Contraseñas no coinciden")]
    public string ConfirmPassword { get; set; }
}
