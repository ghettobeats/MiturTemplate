namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUserClaimsConfig : IEntityTypeConfiguration<AspNetUserClaims>
{
    public void Configure(EntityTypeBuilder<AspNetUserClaims> entity)
    {
        entity.HasIndex(e => e.UserId);

        entity.Property(e => e.UserId).IsRequired();

        entity.HasOne(d => d.User)
            .WithMany(p => p.AspNetUserClaims)
            .HasForeignKey(d => d.UserId);
    }
}