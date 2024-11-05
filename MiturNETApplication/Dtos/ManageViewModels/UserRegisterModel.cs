namespace MiturNetApplication.Dtos.ManageViewModels;
public class UserRegisterModel
{
    [Required(ErrorMessage = "Este campo es requerido")]
    public string UserName { get; set; }
    //[Required]
    //[Display(Name = "Email")]
    //public string Email { get; set; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [StringLength(100, ErrorMessage = "La contraseña debe de tener al menos {2} caracteres.", MinimumLength = 7)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no concuerdan.")]
    public string ConfirmPassword { get; set; }
    //[Required]
    //public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    public string RoleId { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    public string VFirstName { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    public string VLastName { get; set; }
    //[Required]
    //public string Country { get; set; }
    //[Required(ErrorMessage = "Este campo es requerido")]
    public string? VGender { get; set; }
    //[Required]
    //public string Photo { get; set; }
    //[Required]
    //public bool EmailVerificationDisabled { get; set; }
}