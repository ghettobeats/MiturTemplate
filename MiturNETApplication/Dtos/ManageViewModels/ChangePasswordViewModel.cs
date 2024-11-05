namespace MiturNetApplication.Dtos.ManageViewModels;
public class ChangePasswordViewModel
{
    //[Required]
    //[EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [StringLength(100, ErrorMessage = "La contraseña debe de tener al menos {2} caracteres.", MinimumLength = 7)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Ambas contraseñas no coinciden.")]
    //public string ConfirmPassword { get; set; }

    public string Code { get; set; }
    public string UserId { get; set; }
    //[Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    //[Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    public string StatusMessage { get; set; }
}
