namespace MiturNetApplication.Dtos.AccountViewModels;
public class LoginViewModel
{
    [Required]
    //[EmailAddress]
    public string? UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Recuérdame?")]
    public bool RememberMe { get; set; }
}
