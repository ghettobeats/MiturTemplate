namespace MiturNetApplication.Dtos.AccountViewModels;
public class ExternalLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

