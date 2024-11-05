namespace MiturNetApplication.SignalR;
public class HubClient : Hub, IHubClient
{
    private readonly IHubContext<HubClient> _context;   
    public HubClient(IHubContext<HubClient> context)
    {
        _context = context;
    }
    public async Task InformClient(string message)
    {
       await _context.Clients.All.SendAsync("Send", message);
    }

    public async Task SendMessage(string user, string message)
    {
        await _context.Clients.All.SendAsync("ReceiveMessage", user, message);
        
    }
}