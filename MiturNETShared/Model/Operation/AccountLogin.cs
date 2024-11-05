namespace MiturNetShared.Model.Operation;
public class AccountLogin
{
    [Required(ErrorMessage = "Campo requerido")]
    [EmailAddress]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me ?")]
    public bool RememberMe { get; set; }

}
