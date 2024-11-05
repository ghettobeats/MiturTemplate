namespace MiturNetInfrastructure.IdentityConfiguration;

public class SettingConfig : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> entity)
    {
        entity.HasKey(e => e.VSettingId);

        entity.Property(e => e.VSettingId)
            .HasColumnName("vSettingID")
            .HasMaxLength(250)
            .IsUnicode(false);

        entity.Property(e => e.VSettingName)
            .IsRequired()
            .HasColumnName("vSettingName")
            .HasMaxLength(100)
            .IsUnicode(false);

        entity.Property(e => e.VSettingOption)
            .HasColumnName("vSettingOption")
            .HasMaxLength(250)
            .IsUnicode(false);
    }
}