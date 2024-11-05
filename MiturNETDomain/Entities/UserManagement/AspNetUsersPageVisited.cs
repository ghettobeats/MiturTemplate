namespace MiturNetDomain.Entities.UserManagement;
public partial class AspNetUsersPageVisited
{
    public string VPageVisitedId { get; set; }
    public string Id { get; set; }
    public DateTime DDateVisited { get; set; }
    public string NvPageName { get; set; }
    public string NvIpaddress { get; set; }

    public virtual AspNetUsers IdNavigation { get; set; }
}

