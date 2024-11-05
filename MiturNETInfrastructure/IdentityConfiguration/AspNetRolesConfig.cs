namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetRolesConfig : IEntityTypeConfiguration<AspNetRoles>
{
    public void Configure(EntityTypeBuilder<AspNetRoles> entity)
    {

        entity.HasIndex(e => e.NormalizedName)
            .HasDatabaseName("RoleNameIndex")
            .IsUnique()
            .HasFilter("([NormalizedName] IS NOT NULL)");

        entity.Property(e => e.IndexPage)
            .IsRequired()
            .HasMaxLength(250)
            .IsUnicode(false);

        entity.Property(e => e.Name).HasMaxLength(256);

        entity.Property(e => e.NormalizedName).HasMaxLength(256);

    }
}