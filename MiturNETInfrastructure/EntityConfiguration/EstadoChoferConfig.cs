namespace MiturNetInfrastructure.EntityConfiguration;

internal class EstadoChoferConfig : IEntityTypeConfiguration<EstadoChofer>
{
    public void Configure(EntityTypeBuilder<EstadoChofer> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__EstadoCh__3214EC07F6AEF19C");

        entity.ToTable("EstadoChofer");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
        entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
        entity.Property(e => e.CreadoEn).HasColumnType("datetime");
        entity.Property(e => e.CreadoPor).HasMaxLength(255);
        entity.Property(e => e.Nombre).HasMaxLength(255);

        entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.EstadosChofer)
            .HasForeignKey(d => d.IdEstado)
            .HasConstraintName("FK__EstadoCho__IdEst__7F2BE32F");
    }
}
