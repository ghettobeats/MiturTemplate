namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetRoleClaimsConfig : IEntityTypeConfiguration<AspNetRoleClaims>
{
    public void Configure(EntityTypeBuilder<AspNetRoleClaims> entity)
    {
            entity.HasIndex(e => e.RoleId);

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role)
                .WithMany(p => p.AspNetRoleClaims)
                .HasForeignKey(d => d.RoleId);
    }
}