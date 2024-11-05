
namespace MiturNetApplication.Dtos.ManageViewModels;
public class RoleWithMenuPermission
{
    public AspNetRoles Role { get; set; }
    public List<AspNetUsersMenuPermission> MenuPermission { get; set; }
}
