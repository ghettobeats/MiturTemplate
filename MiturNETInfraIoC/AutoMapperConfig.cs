namespace MiturNetInfraIoC;
public static class AutoMapperConfig
{
    public static void AddAutoMapperConfiguration(
        this IServiceCollection services, Assembly assembly)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

    }
}




