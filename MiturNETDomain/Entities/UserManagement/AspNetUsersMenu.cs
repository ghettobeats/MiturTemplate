namespace MiturNetDomain.Entities.UserManagement;
public partial class AspNetUsersMenu
{
    public AspNetUsersMenu()
    {
        AspNetUsersMenuPermission = new HashSet<AspNetUsersMenuPermission>();
        InverseVParentMenu = new HashSet<AspNetUsersMenu>();
    }

    public string VMenuId { get; set; }
    public string NvMenuName { get; set; }
    public int ISerialNo { get; set; }
    public string NvFabIcon { get; set; }
    public string? VParentMenuId { get; set; }
    public string NvPageUrl { get; set; }

    public virtual AspNetUsersMenu VParentMenu { get; set; }
    public virtual ICollection<AspNetUsersMenuPermission> AspNetUsersMenuPermission { get; set; }
    public virtual ICollection<AspNetUsersMenu> InverseVParentMenu { get; set; }
}
