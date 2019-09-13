using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.ASPNETCore.Web.Hubs
{
    public class RandomNumberHub : Hub
    {
        public RandomNumberHub()
        {
        }

        public async Task GetRandomNumbers(Guid requestId, int count, int delayInMs)
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, requestId.ToString());

            await Task.Run(async () =>
            {
                for (var counter = 0; counter < count; counter++)
                {
                    await Task.Delay(delayInMs);
                    await Clients.Client(Context.ConnectionId).SendAsync("Receive", requestId, new Random().NextDouble());
                }
            });

            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, requestId.ToString());
        }

    }
}
