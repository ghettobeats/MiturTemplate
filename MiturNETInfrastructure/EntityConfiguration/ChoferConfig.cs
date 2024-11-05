namespace MiturNetInfrastructure.EntityConfiguration;
    internal class ChoferConfig : IEntityTypeConfiguration<Chofer>
    {
        public void Configure(EntityTypeBuilder<Chofer> entity)
        {
      
            entity.HasKey(e => e.Id).HasName("PK__Chofer__3214EC07984A763F");

            entity.ToTable("Chofer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ActualizadoEn).HasColumnType("datetime");
            entity.Property(e => e.ActualizadoPor).HasMaxLength(255);
            entity.Property(e => e.Apellido).HasMaxLength(255);
            entity.Property(e => e.Cedula).HasMaxLength(255);
            entity.Property(e => e.CreadoEn).HasColumnType("datetime");
            entity.Property(e => e.CreadoPor).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IdEmpleado).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.Telefono).HasMaxLength(255);

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Choferes)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Chofer__IdEstado__7E37BEF6");

            entity.HasOne(d => d.IdEstadoChoferNavigation).WithMany(p => p.Choferes)
                .HasForeignKey(d => d.IdEstadoChofer)
                .HasConstraintName("FK__Chofer__IdEstado__7D439ABD");
        }
}


