﻿using Microsoft.AspNetCore.SignalR;

namespace UPOD.API.HubService
{
    public class NotifyHub: Hub
    {
        public async Task SendNotify(List<Guid> userId, string message)
        {
            foreach (var item in userId)
            {
                await Clients.All.SendAsync("NotifyMessage", item, message);
            }
        }
    }
}
