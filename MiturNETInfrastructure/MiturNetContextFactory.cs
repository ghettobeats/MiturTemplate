namespace MiturNetInfrastructure;

public class MiturNetContextFactory : IDesignTimeDbContextFactory<MiturNetContext>
{
    public MiturNetContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MiturNetContext>();
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MiturNet;Integrated Security=True;TrustServerCertificate=true");

        return new MiturNetContext(optionsBuilder.Options, null);
    }
}
