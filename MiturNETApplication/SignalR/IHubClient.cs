namespace MiturNetApplication.SignalR;
public interface IHubClient
{
    Task InformClient(string message);
    Task SendMessage(string user, string message);
}
