namespace MiturNetInfrastructure.EntityConfiguration;

internal class EstadoSolicitudConfig : IEntityTypeConfiguration<EstadoSolicitud>
{
    public void Configure(EntityTypeBuilder<EstadoSolicitud> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__EstadoSo__3214EC07185155CA");

        entity.ToTable("EstadoSolicitud");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
        entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
        entity.Property(e => e.CreadoEn).HasColumnType("datetime");
        entity.Property(e => e.CreadoPor).HasMaxLength(255);
        entity.Property(e => e.Nombre).HasMaxLength(255);

        entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.EstadosSolicitudes)
            .HasForeignKey(d => d.IdEstado)
            .HasConstraintName("FK__EstadoSol__IdEst__00200768");
    }
}
