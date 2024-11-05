namespace MiturNetApplication.Dtos.ManageViewModels;
public class ProfileViewModel
{
    [Required]
    public string Id { get; set; }
    //[Required]
    //[EmailAddress]
    //public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    //[Required]
    //public string PhoneNumber { get; set; }
    [Required]
    public string RoleId { get; set; }
    [Required]
    public string VFirstName { get; set; }
    [Required]
    public string VLastName { get; set; }
    //[Required]
    //public string Country { get; set; }
    
    public string? VGender { get; set; }
    //[Required]
    //public string Photo { get; set; }
}