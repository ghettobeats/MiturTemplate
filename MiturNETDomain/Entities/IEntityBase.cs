namespace MiturNetDomain.Entities;

public interface IEntityBase
{
    public int id { get; set; }
    public string CreatedBy { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public Guid TenantId { get; set; }
}
