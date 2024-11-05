
namespace MiturNetInfrastructure.EntityConfiguration;

internal class SolicitudConfig : IEntityTypeConfiguration<Solicitud>
{
    public void Configure(EntityTypeBuilder<Solicitud> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC07571202BD");

        entity.ToTable("Solicitud");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
        entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
        entity.Property(e => e.Adjunto)
            .HasMaxLength(1)
            .IsFixedLength();
        entity.Property(e => e.Comentario).HasMaxLength(255);
        entity.Property(e => e.CreadoEn).HasColumnType("datetime");
        entity.Property(e => e.CreadoPor).HasMaxLength(255);
        entity.Property(e => e.Destino).HasMaxLength(255);
        entity.Property(e => e.FechaFin).HasColumnType("datetime");
        entity.Property(e => e.FechaInicio).HasColumnType("datetime");
        entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
        entity.Property(e => e.NumeroDocumento).HasMaxLength(255);
        entity.Property(e => e.Proposito).HasMaxLength(255);

        entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Solicitudes)
            .HasForeignKey(d => d.IdEstado)
            .HasConstraintName("FK__Solicitud__IdEst__02FC7413");

        entity.HasOne(d => d.IdEstadoSolicitudNavigation).WithMany(p => p.Solicitudes)
            .HasForeignKey(d => d.IdEstadoSolicitud)
            .HasConstraintName("FK__Solicitud__IdEst__02084FDA");
    }
}