namespace MiturNetInfrastructure.EntityConfiguration;
public class ScalarValueConfig : IEntityTypeConfiguration<ScalarValue>
{
    public void Configure(EntityTypeBuilder<ScalarValue> entity)
    {
        entity.HasNoKey();
    }
}