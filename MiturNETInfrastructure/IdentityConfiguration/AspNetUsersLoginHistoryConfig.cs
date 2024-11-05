namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUsersLoginHistoryConfig : IEntityTypeConfiguration<AspNetUsersLoginHistory>
{
    public void Configure(EntityTypeBuilder<AspNetUsersLoginHistory> entity)
    {
            entity.HasKey(e => e.VUlhid);

            entity.Property(e => e.VUlhid)
                .HasColumnName("vULHID")
                .HasMaxLength(250);

            entity.Property(e => e.DLogIn)
                .HasColumnName("dLogIn")
                .HasColumnType("datetime");

            entity.Property(e => e.DLogOut)
                .HasColumnName("dLogOut")
                .HasColumnType("datetime");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(e => e.NvIpaddress)
                .IsRequired()
                .HasColumnName("nvIPAddress");

        entity.HasOne(d => d.IdNavigation)
            .WithMany(p => p.AspNetUsersLoginHistory)
            .HasForeignKey(d => d.Id)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
