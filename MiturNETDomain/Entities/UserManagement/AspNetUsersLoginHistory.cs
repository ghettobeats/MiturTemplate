namespace MiturNetDomain.Entities.UserManagement;
public partial class AspNetUsersLoginHistory
{
    public string VUlhid { get; set; }
    public string Id { get; set; }
    public DateTime DLogIn { get; set; }
    public DateTime? DLogOut { get; set; }
    public string NvIpaddress { get; set; }

    public virtual AspNetUsers IdNavigation { get; set; }
}
