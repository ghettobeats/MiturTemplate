namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUserTokensConfig : IEntityTypeConfiguration<AspNetUserTokens>
{
    public void Configure(EntityTypeBuilder<AspNetUserTokens> entity)
    {
        entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        entity.Property(e => e.LoginProvider).HasMaxLength(128);

        entity.Property(e => e.Name).HasMaxLength(128);

        entity.HasOne(d => d.User)
            .WithMany(p => p.AspNetUserTokens)
            .HasForeignKey(d => d.UserId);
    }
}