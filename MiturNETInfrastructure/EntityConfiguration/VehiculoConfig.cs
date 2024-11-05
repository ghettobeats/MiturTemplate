namespace MiturNetInfrastructure.EntityConfiguration;

internal class VehiculoConfig : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__Vehiculo__3214EC076A1346CD");

        entity.ToTable("Vehiculo");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
        entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
        entity.Property(e => e.CreadoEn).HasColumnType("datetime");
        entity.Property(e => e.CreadoPor).HasMaxLength(255);
        entity.Property(e => e.Marca).HasMaxLength(255);
        entity.Property(e => e.Modelo).HasMaxLength(255);
        entity.Property(e => e.Placa).HasMaxLength(255);

        entity.HasOne(d => d.IdChoferNavigation).WithMany(p => p.Vehiculos)
            .HasForeignKey(d => d.IdChofer)
            .HasConstraintName("FK__Vehiculo__IdChof__06CD04F7");

        entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Vehiculos)
            .HasForeignKey(d => d.IdEstado)
            .HasConstraintName("FK__Vehiculo__IdEsta__08B54D69");

        entity.HasOne(d => d.IdEstadoVehiculoNavigation).WithMany(p => p.Vehiculos)
            .HasForeignKey(d => d.IdEstadoVehiculo)
            .HasConstraintName("FK__Vehiculo__IdEsta__07C12930");
    }
}