
namespace MiturNetShared.Model.Operation;

public class AspNetUsersProfileZona
{
    public string Id { get; set; }
    public string VFirstName { get; set; }
    public string VLastName { get; set; }
    public string? VCountry { get; set; }
    public string? VGender { get; set; }
    public string? VPhoto { get; set; }
    public IdNavigation IdNavigation { get; set; }
}
