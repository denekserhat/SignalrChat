using Microsoft.AspNetCore.SignalR;
using SIGNALRCHAT.Context;
using SIGNALRCHAT.Models;

namespace SIGNALRCHAT.Hubs;

public sealed class ChatHub(ApplicationDbContext context) : Hub
{
    public static Dictionary<string, Guid> Users = new();
    public async Task Connect(Guid userId)
    {
        Users.Add(Context.ConnectionId, userId);
        User? user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        user.Status = "online";
        await context.SaveChangesAsync();

        await Clients.All.SendAsync("Users", user);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Users.TryGetValue(Context.ConnectionId, out Guid userId);

        User? user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        user.Status = "offline";
        await context.SaveChangesAsync();
    }
}
