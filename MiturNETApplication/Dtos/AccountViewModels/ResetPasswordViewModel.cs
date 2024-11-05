namespace MiturNetApplication.Dtos.AccountViewModels;
public class ResetPasswordViewModel
{
    [Required]
    public string id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "El {0} debe de ser por lo menos {2} y como máximo {1} carácteres de longitud.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Contraseña")]
    [Compare("Password", ErrorMessage = "Las Contraseñas no coinciden")]
    public string ConfirmPassword { get; set; }

    //[Required]
    //public string Code { get; set; }
}
