namespace MiturNetDomain.Entities.UserManagement;
public partial class AspNetUsersProfile
{
    public string Id { get; set; }
    public string VFirstName { get; set; }
    public string VLastName { get; set; }
    public string? VCountry { get; set; }
    public string? VGender { get; set; }
    public string? VPhoto { get; set; }

    public virtual AspNetUsers IdNavigation { get; set; }
   

}
