namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUsersMenuConfig : IEntityTypeConfiguration<AspNetUsersMenu>
{
    public void Configure(EntityTypeBuilder<AspNetUsersMenu> entity)
    {
        entity.HasKey(e => e.VMenuId);

        entity.Property(e => e.VMenuId)
            .HasColumnName("vMenuID")
            .HasMaxLength(250)
            .IsUnicode(false);

        entity.Property(e => e.ISerialNo).HasColumnName("iSerialNo");

        entity.Property(e => e.NvFabIcon)
            .IsRequired()
            .HasColumnName("nvFabIcon");

        entity.Property(e => e.NvMenuName)
            .IsRequired()
            .HasColumnName("nvMenuName");

        entity.Property(e => e.NvPageUrl)
            .IsRequired()
            .HasColumnName("nvPageUrl");

        entity.Property(e => e.VParentMenuId)
            .HasColumnName("vParentMenuID")
            .HasMaxLength(250)
            .IsUnicode(false);

        entity.HasOne(d => d.VParentMenu)
            .WithMany(p => p.InverseVParentMenu)
            .HasForeignKey(d => d.VParentMenuId)
            .HasConstraintName("FK_AspNetUsersMenu_AspNetUsersMenu");
    }
}