namespace MiturNetInfrastructure.EntityConfiguration;

internal class EstadoConfig : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__Estado__3214EC071945AB43");

        entity.ToTable("Estado");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
        entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
        entity.Property(e => e.CreadoEn).HasColumnType("datetime");
        entity.Property(e => e.CreadoPor).HasMaxLength(255);
        entity.Property(e => e.Descripcion).HasMaxLength(255);
        entity.Property(e => e.NombreEstado).HasMaxLength(255);
    }
}
