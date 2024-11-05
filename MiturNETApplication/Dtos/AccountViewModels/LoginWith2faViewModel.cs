namespace MiturNetApplication.Dtos.AccountViewModels;
public class LoginWith2faViewModel
{
    [Required]
    [StringLength(7, ErrorMessage = "El {0} debe de ser por lo menos {2} y como máximo {1} carácteres de longitud.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Authenticator code")]
    public string TwoFactorCode { get; set; }

    [Display(Name = "Recordar este dispositivo")]
    public bool RememberMachine { get; set; }

    public bool RememberMe { get; set; }
}
