namespace MiturNetDomain.Entities.UserManagement;
public partial class AspNetRoles
{
    public AspNetRoles()
    {
        AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
        AspNetUserRoles = new HashSet<AspNetUserRoles>();
        AspNetUsersMenuPermission = new HashSet<AspNetUsersMenuPermission>();
    }

    public string Id { get; set; }
    public string? Name { get; set; }
    public string? NormalizedName { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public string IndexPage { get; set; }

    public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
    public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    public virtual ICollection<AspNetUsersMenuPermission> AspNetUsersMenuPermission { get; set; }
}
