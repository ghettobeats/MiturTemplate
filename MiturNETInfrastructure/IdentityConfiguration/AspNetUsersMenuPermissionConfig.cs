namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUsersMenuPermissionConfig : IEntityTypeConfiguration<AspNetUsersMenuPermission>
{
    public void Configure(EntityTypeBuilder<AspNetUsersMenuPermission> entity)
    {
        entity.HasKey(e => e.VMenuPermissionId);

        entity.HasIndex(e => new { e.VMenuId, e.Id })
            .HasDatabaseName("IX_AspNetUsersMenuPermission")
            .IsUnique();

        entity.Property(e => e.VMenuPermissionId)
            .HasColumnName("vMenuPermissionID")
            .HasMaxLength(50)
            .IsUnicode(false);

        entity.Property(e => e.Id).IsRequired();

        entity.Property(e => e.VMenuId)
            .IsRequired()
            .HasColumnName("vMenuID")
            .HasMaxLength(250)
            .IsUnicode(false);

        entity.HasOne(d => d.IdNavigation)
            .WithMany(p => p.AspNetUsersMenuPermission)
            .HasForeignKey(d => d.Id)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AspNetUsersMenuPermission_AspNetRoles");

        entity.HasOne(d => d.VMenu)
            .WithMany(p => p.AspNetUsersMenuPermission)
            .HasForeignKey(d => d.VMenuId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AspNetUsersMenuPermission_AspNetUsersMenu");
    }
}
