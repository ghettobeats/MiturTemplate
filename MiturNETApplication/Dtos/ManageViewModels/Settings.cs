namespace MiturNetApplication.Dtos.ManageViewModels;
public class Settings
{
    public bool UserRegister { get; set; }
    public bool EmailVerificationDisable { get; set; }
    [Required]
    public string UserRole { get; set; }
    public bool RecoverPassword { get; set; }
    public bool ChangePassword { get; set; }
    public bool ChangeProfile { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool ExternalLogin { get; set; }
}