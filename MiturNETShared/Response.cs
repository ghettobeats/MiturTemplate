namespace MiturNetShared;
public class Response<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public bool Succes { get; set; }
}