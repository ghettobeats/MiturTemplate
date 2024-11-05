namespace MiturNetInfrastructure.DBContext;

public partial class MiturNetContext : DbContext
{
    private IHttpContextAccessor _httpContextAccessor;
    public MiturNetContext(DbContextOptions<MiturNetContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
       _httpContextAccessor = httpContextAccessor;
    }

    //Identity
    public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
    public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
    public virtual DbSet<AspNetUsersLoginHistory> AspNetUsersLoginHistory { get; set; }
    public virtual DbSet<AspNetUsersMenu> AspNetUsersMenus { get; set; }
    public virtual DbSet<AspNetUsersMenuPermission> AspNetUsersMenuPermission { get; set; }
    public virtual DbSet<AspNetUsersPageVisited> AspNetUsersPageVisited { get; set; }
    public virtual DbSet<AspNetUsersProfile> AspNetUsersProfile { get; set; }
    public virtual DbSet<Setting> Setting { get; set; }
    public virtual DbSet<Chofer> Choferes { get; set; }
    public virtual DbSet<Estado> Estados { get; set; }
    public virtual DbSet<EstadoChofer> EstadosChofer { get; set; }
    public virtual DbSet<EstadoSolicitud> EstadoSolicitudes { get; set; }
    public virtual DbSet<EstadoVehiculo> EstadoVehiculos { get; set; }
    public virtual DbSet<Solicitud> Solicitudes { get; set; }
    public virtual DbSet<SolicitudDetalle> SolicitudDetalles { get; set; }
    public virtual DbSet<Vehiculo> Vehiculos { get; set; }



    //Stored procedure //


    //Vista //
    //public virtual DbSet<VistaUsuario> VistaUsuario { get; set; }



    //Tablas



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(getDBConnection.Connexion);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfiguration(new AspNetRoleClaimsConfig());
        modelBuilder.ApplyConfiguration(new AspNetRolesConfig());
        modelBuilder.ApplyConfiguration(new AspNetUserClaimsConfig());
        modelBuilder.ApplyConfiguration(new AspNetUserLoginsConfig());
        modelBuilder.ApplyConfiguration(new AspNetUserRolesConfig());
        modelBuilder.ApplyConfiguration(new AspNetUsersConfig());
        modelBuilder.ApplyConfiguration(new AspNetUsersLoginHistoryConfig());
        modelBuilder.ApplyConfiguration(new AspNetUsersMenuConfig());
        modelBuilder.ApplyConfiguration(new AspNetUsersMenuPermissionConfig());
        modelBuilder.ApplyConfiguration(new AspNetUsersPageVisitedConfig());
        modelBuilder.ApplyConfiguration(new AspNetUsersProfileConfig());
        modelBuilder.ApplyConfiguration(new AspNetUserTokensConfig());
        modelBuilder.ApplyConfiguration(new SettingConfig());
        modelBuilder.ApplyConfiguration(new ChoferConfig());
        modelBuilder.ApplyConfiguration(new EstadoConfig());
        modelBuilder.ApplyConfiguration(new EstadoSolicitudConfig());
        modelBuilder.ApplyConfiguration(new EstadoVehiculoConfig());
        modelBuilder.ApplyConfiguration(new SolicitudConfig());
        modelBuilder.ApplyConfiguration(new SolicitudDetalleConfig());
        modelBuilder.ApplyConfiguration(new VehiculoConfig());

        //Stored Procedure //


        //Vistas //
        //modelBuilder.ApplyConfiguration(new VistaUsuarioConfig());



        //Tablas //
        //modelBuilder.ApplyConfiguration(new EstatusConfig());



        //OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public override int SaveChanges()
    {
        // Get all the entities that inherit from AuditableEntity
        // and have a state of Added or Modified
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is EntityBase && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        // For each entity we will set the Audit properties
        foreach (var entityEntry in entries)
        {
            // If the entity state is Added let's set
            // the CreatedAt and CreatedBy properties
            if (entityEntry.State == EntityState.Added)
            {
                ((EntityBase)entityEntry.Entity).CreadoEn = DateTime.UtcNow;
                ((EntityBase)entityEntry.Entity).CreadoPor = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "MyApp";
                //.FindFirst(JwtRegisteredClaimNames.Sub).Value .FindFirstValue(ClaimTypes.NameIdentifier) ?? "MyApp";
            }
            else
            {
                // If the state is Modified then we don't want
                // to modify the CreatedAt and CreatedBy properties
                // so we set their state as IsModified to false
                Entry((EntityBase)entityEntry.Entity).Property(p => p.CreadoEn).IsModified = false;
                Entry((EntityBase)entityEntry.Entity).Property(p => p.CreadoPor).IsModified = false;
            }

            // In any case we always want to set the properties
            // ModifiedAt and ModifiedBy
            ((EntityBase)entityEntry.Entity).ActualizadoEn = DateTime.UtcNow;
            ((EntityBase)entityEntry.Entity).ActualizadoPor = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "MyApp";
        }

        // After we set all the needed properties
        // we call the base implementation of SaveChanges
        // to actually save our entities in the database
        return base.SaveChanges();
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Get all the entities that inherit from AuditableEntity
        // and have a state of Added or Modified
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is EntityBase && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        // For each entity we will set the Audit properties
        foreach (var entityEntry in entries)
        {
            // If the entity state is Added let's set
            // the CreatedAt and CreatedBy properties
            if (entityEntry.State == EntityState.Added)
            {
                ((EntityBase)entityEntry.Entity).CreadoEn = DateTime.UtcNow;
                ((EntityBase)entityEntry.Entity).CreadoPor = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "DefaultUser";
            }
            else
            {
                // If the state is Modified then we don't want
                // to modify the CreatedAt and CreatedBy properties
                // so we set their state as IsModified to false
                Entry((EntityBase)entityEntry.Entity).Property(p => p.CreadoEn).IsModified = false;
                Entry((EntityBase)entityEntry.Entity).Property(p => p.CreadoPor).IsModified = false;
            }

            // In any case we always want to set the properties
            // ModifiedAt and ModifiedBy
            ((EntityBase)entityEntry.Entity).ActualizadoEn = DateTime.UtcNow;
            ((EntityBase)entityEntry.Entity).ActualizadoPor = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "DefaultUserUpdated";
        }

        // After we set all the needed properties
        // we call the base implementation of SaveChangesAsync
        // to actually save our entities in the database
        return await base.SaveChangesAsync(cancellationToken);
    }
   

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
    }

}

