namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUserLoginsConfig : IEntityTypeConfiguration<AspNetUserLogins>
{
    public void Configure(EntityTypeBuilder<AspNetUserLogins> entity)
    {
        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

        entity.HasIndex(e => e.UserId);

        entity.Property(e => e.LoginProvider).HasMaxLength(128);

        entity.Property(e => e.ProviderKey).HasMaxLength(128);

        entity.Property(e => e.UserId).IsRequired();

        entity.HasOne(d => d.User)
            .WithMany(p => p.AspNetUserLogins)
            .HasForeignKey(d => d.UserId);
    }
}