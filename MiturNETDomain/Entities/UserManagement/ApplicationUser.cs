namespace MiturNetDomain.Entities.UserManagement;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
}
