
namespace MiturNetInfrastructure.EntityConfiguration;

internal class SolicitudDetalleConfig : IEntityTypeConfiguration<SolicitudDetalle>
{
    public void Configure(EntityTypeBuilder<SolicitudDetalle> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC07FC81005B");

        entity.ToTable("SolicitudDetalle");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
        entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
        entity.Property(e => e.CreadoEn).HasColumnType("datetime");
        entity.Property(e => e.CreadoPor).HasMaxLength(255);

        entity.HasOne(d => d.IdChoferNavigation).WithMany(p => p.SolicitudDetalles)
            .HasForeignKey(d => d.IdChofer)
            .HasConstraintName("FK_SolicitudDetalle_Chofer");

        entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.SolicitudDetalles)
            .HasForeignKey(d => d.IdEstado)
            .HasConstraintName("FK__Solicitud__IdEst__03F0984C");

        entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.SolicitudDetalles)
            .HasForeignKey(d => d.IdSolicitud)
            .HasConstraintName("FK__Solicitud__IdSol__04E4BC85");
    }
}