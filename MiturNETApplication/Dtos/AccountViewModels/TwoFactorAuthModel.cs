namespace MiturNetApplication.Dtos.AccountViewModels;
public class TwoFactorAuthModel
{
    [Required]
    public string TFACode { get; set; }
    public string Id { get; set; }
}
