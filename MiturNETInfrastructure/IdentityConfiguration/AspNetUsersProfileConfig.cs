namespace MiturNetInfrastructure.IdentityConfiguration;

public class AspNetUsersProfileConfig : IEntityTypeConfiguration<AspNetUsersProfile>
{
    public void Configure(EntityTypeBuilder<AspNetUsersProfile> entity)
    {
            entity.Property(e => e.VCountry)
                  .HasColumnName("vCountry");

            entity.Property(e => e.VFirstName)
                .IsRequired()
                .HasColumnName("vFirstName")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.VGender)
                .HasColumnName("vGender");

            entity.Property(e => e.VLastName)
                .IsRequired()
                .HasColumnName("vLastName")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.VPhoto)
                 .HasColumnName("vPhoto");

        entity.HasOne(d => d.IdNavigation)
            .WithOne(p => p.AspNetUsersProfile)
        .HasForeignKey<AspNetUsersProfile>(d => d.Id)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("FK_AspNetUsersProfile_AspNetUsers");



    }
}