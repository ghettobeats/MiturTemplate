namespace MiturNetInfraIoC;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       // var domain = configuration.GetSection("AppSettings")["Domain"];
       // var token = configuration.GetSection("AppSettings")["Token"];
        getDBConnection.Connexion = configuration.GetConnectionString("DBConnection");

        services
            .AddScoped(
                typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddTransient(
                typeof(IServiceBase<>), typeof(ServiceBase<>))
            .AddTransient(
                typeof(IServiceNoEntity<>), typeof(ServiceNoEntity<>))
            //.AddTransient<IHubClient, HubClient>()
            .AddTransient<IEmailSender, EmailSender>();
        services.AddHttpContextAccessor();

        services.AddDbContext<MiturNetContext>(options =>
        {
            options.UseSqlServer(getDBConnection.Connexion,
            sqlServerOptions => sqlServerOptions.CommandTimeout(7200));
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(getDBConnection.Connexion,
                sqlServerOptions => sqlServerOptions.CommandTimeout(7200));
        });

        //services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
   
        // Add services to the container.
        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                opt.JsonSerializerOptions.Converters.Add(new DateConverter());
            });        

        services.AddHttpContextAccessor();
        services.AddAutoMapperConfiguration(Assembly.GetExecutingAssembly());

        services.Configure<IdentityOptions>(options =>
         {
             // Password settings
             options.Password.RequireDigit = true;
             options.Password.RequiredLength = 7;
             options.Password.RequireNonAlphanumeric = false;
             options.Password.RequireUppercase = true;
             options.Password.RequireLowercase = false;
             options.Password.RequiredUniqueChars = 6;

             // Lockout settings
             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
             options.Lockout.MaxFailedAccessAttempts = 10;
             options.Lockout.AllowedForNewUsers = true;

             // Default SignIn settings.
             options.SignIn.RequireConfirmedEmail = false;
             //options.SignIn.RequireConfirmedPhoneNumber = false;
             //options.User.RequireUniqueEmail = true;
         });

        //services.AddCors(options =>
        // {
        //     options.AddPolicy("CorsPolicy",
        //         builder => builder
        //         .WithOrigins("http://localhost:90/SegasaRadzenAPI/hubClient")
        //         //.AllowAnyOrigin()
        //         .AllowAnyMethod()
        //         .AllowAnyHeader()
        //         .AllowCredentials()
        //         .SetIsOriginAllowed((hosts) => true));
        // });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiturNet", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization."

            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
        });

        //Add SignalR
        services.AddSignalR(opt => { 
            opt.EnableDetailedErrors = true; 
            opt.HandshakeTimeout = TimeSpan.FromSeconds(600); 
        }); 

        services.AddResponseCompression(opt =>
        {
            opt.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/octet-stream" });
        });

        //Initialize JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddJwtBearer(jwtBearerOptions =>
        {
            //
            jwtBearerOptions.RequireHttpsMetadata = false;
            jwtBearerOptions.SaveToken = true;
            //
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration["AppSettings:Issuer"],
                ValidAudience = configuration["AppSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"])),
                //RESET
                ClockSkew = TimeSpan.Zero
            };
        }).AddIdentityCookies(o => { });

        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme);

            defaultAuthorizationPolicyBuilder =
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });

        services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();

        return services;
    }



}

