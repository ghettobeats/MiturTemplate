namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUserRolesConfig : IEntityTypeConfiguration<AspNetUserRoles>
{
    public void Configure(EntityTypeBuilder<AspNetUserRoles> entity)
    {
        entity.HasKey(e => new { e.UserId, e.RoleId });

        entity.HasIndex(e => e.RoleId);

        entity.HasOne(d => d.Role)
            .WithMany(p => p.AspNetUserRoles)
            .HasForeignKey(d => d.RoleId);

        entity.HasOne(d => d.User)
            .WithMany(p => p.AspNetUserRoles)
            .HasForeignKey(d => d.UserId);

    }
}