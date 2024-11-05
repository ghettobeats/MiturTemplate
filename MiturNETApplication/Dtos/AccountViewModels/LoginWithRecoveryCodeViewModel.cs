namespace MiturNetApplication.Dtos.AccountViewModels;
public class LoginWithRecoveryCodeViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Codigo de Recuperacón")]
    public string RecoveryCode { get; set; }
}
