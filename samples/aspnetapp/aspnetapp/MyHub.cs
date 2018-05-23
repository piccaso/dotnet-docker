using System;
using System.Collections.Generic;
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

        public override async Task OnConnectedAsync()
        {
            StaticHub.Users.Add(Context.ConnectionId);
            await SendMessage(StaticHub.Users.Count.ToString(), "\n" + string.Join("\n", StaticHub.Users));
            //base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                StaticHub.Users.Remove(Context.ConnectionId);
                await SendMessage(StaticHub.Users.Count.ToString(), "\n" + string.Join("\n", StaticHub.Users));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}