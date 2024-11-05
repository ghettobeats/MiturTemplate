namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUsersConfig : IEntityTypeConfiguration<AspNetUsers>
{
    public void Configure(EntityTypeBuilder<AspNetUsers> entity)
    {
        entity.HasIndex(e => e.NormalizedEmail).HasDatabaseName("EmailIndex");

        entity.HasIndex(e => e.NormalizedUserName)
            .HasDatabaseName("UserNameIndex")
            .IsUnique()
            .HasFilter("([NormalizedUserName] IS NOT NULL)");

        entity.Property(e => e.Date)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");

        entity.Property(e => e.Email).HasMaxLength(256);

        entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

        entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

        entity.Property(e => e.UserName).HasMaxLength(256);
    }
}