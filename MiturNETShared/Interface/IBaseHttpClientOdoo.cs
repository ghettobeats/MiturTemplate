namespace MiturNetShared.Interface;
public interface IBaseHttpClientOdoo
{
    Task<T> MakeRequest<T>(string httpMethod, string route, Dictionary<string, string> postParams = null, CancellationToken cancellationToken = default);
}
