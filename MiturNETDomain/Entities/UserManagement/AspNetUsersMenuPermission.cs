namespace MiturNetDomain.Entities.UserManagement;
public partial class AspNetUsersMenuPermission
{
    public string VMenuPermissionId { get; set; }
    public string Id { get; set; }
    public string VMenuId { get; set; }

    public virtual AspNetRoles IdNavigation { get; set; }
    public virtual AspNetUsersMenu VMenu { get; set; }
}

