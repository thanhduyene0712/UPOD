using Microsoft.AspNetCore.SignalR;

namespace UPOD.API.HubServices
{
    public class NotificationsHub: Hub
    {
        public async Task SendNotify(string user, string message)
        {
            await Clients.All.SendAsync("NotifyMessage", user, message);
        }
    }
}
