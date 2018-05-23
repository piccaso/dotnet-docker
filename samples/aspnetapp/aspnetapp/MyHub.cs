using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace aspnetapp
{

    public static class StaticHub
    {
        public static HashSet<string> Users = new HashSet<string>();
    }

    public class MyHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendUpdates()
        {
            int userCount = 0;
            string[] users = null;
            lock (StaticHub.Users)
            {
                userCount = StaticHub.Users.Count;
                users = StaticHub.Users.ToArray();
            }
            await Clients.All.SendAsync("UserCount", userCount);
            await Clients.All.SendAsync("Users", users);
        }

        public override async Task OnConnectedAsync()
        {
            lock (StaticHub.Users)
            {
                StaticHub.Users.Add(Context.ConnectionId);
            }
            await SendUpdates();
            //base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                lock (StaticHub.Users)
                {
                    StaticHub.Users.Remove(Context.ConnectionId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            await SendUpdates();
        }
    }
}