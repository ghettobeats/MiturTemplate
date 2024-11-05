namespace MiturNetShared.Services;

public class PropertyChangedEventArgs
{
    public string Name { get; set; }
    public object NewValue { get; set; }
    public object OldValue { get; set; }
    public bool IsGlobal { get; set; }
}
