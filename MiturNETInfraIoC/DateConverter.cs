namespace MiturNetInfraIoC;
public class DateConverter : JsonConverter<DateOnly>
{
    private string formatDate = "dd/MM/yyyy";
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.Parse(reader.GetString());   
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(formatDate));
    }
}
