
namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUsersPageVisitedConfig : IEntityTypeConfiguration<AspNetUsersPageVisited>
{
    public void Configure(EntityTypeBuilder<AspNetUsersPageVisited> entity)
    {
        entity.HasKey(e => e.VPageVisitedId);

        entity.Property(e => e.VPageVisitedId)
            .HasColumnName("vPageVisitedID")
            .HasMaxLength(128);

        entity.Property(e => e.DDateVisited)
            .HasColumnName("dDateVisited")
            .HasColumnType("datetime");

        entity.Property(e => e.Id)
            .IsRequired()
            .HasMaxLength(450);

        entity.Property(e => e.NvIpaddress)
            .IsRequired()
            .HasColumnName("nvIPAddress");

        entity.Property(e => e.NvPageName)
            .IsRequired()
            .HasColumnName("nvPageName");

        entity.HasOne(d => d.IdNavigation)
            .WithMany(p => p.AspNetUsersPageVisited)
            .HasForeignKey(d => d.Id)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AspNetUsersPageVisited_AspNetUsers");
    }
}