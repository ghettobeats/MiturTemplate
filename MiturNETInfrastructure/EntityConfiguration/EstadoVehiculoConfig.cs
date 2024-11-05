
namespace MiturNetInfrastructure.EntityConfiguration;

internal class EstadoVehiculoConfig : IEntityTypeConfiguration<EstadoVehiculo>
{
    public void Configure(EntityTypeBuilder<EstadoVehiculo> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__EstadoVe__3214EC07DF46176B");

        entity.ToTable("EstadoVehiculo");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
        entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
        entity.Property(e => e.CreadoEn).HasColumnType("datetime");
        entity.Property(e => e.CreadoPor).HasMaxLength(255);
        entity.Property(e => e.Nombre).HasMaxLength(255);

        entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.EstadosVehiculos)
            .HasForeignKey(d => d.IdEstado)
            .HasConstraintName("FK__EstadoVeh__IdEst__01142BA1");
    }
}
