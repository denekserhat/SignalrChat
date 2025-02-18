using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SIGNALRCHAT.Context;
using SIGNALRCHAT.Dtos;
using SIGNALRCHAT.Hubs;
using SIGNALRCHAT.Models;

namespace SIGNALRCHAT.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class ChatsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatsController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }
    [HttpGet]
    public async Task<IActionResult> GetChats(Guid userId, Guid toUserId, CancellationToken cancellationToken)
    {
        var chats = await _context.Chats
            .Where(x => x.UserId == userId || x.ToUserId == toUserId || x.UserId == toUserId || x.ToUserId == userId)
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);
        return Ok(chats);
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage(SendMessageDto request, CancellationToken cancellationToken)
    {
        Chat chat = new()
        {
            UserId = request.UserId,
            ToUserId = request.ToUserId,
            Message = request.Message,
            Date = DateTime.UtcNow
        };

        await _context.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        string connectionId = ChatHub.Users.First(x => x.Value == chat.ToUserId).Key;
        await _hubContext.Clients.Client(connectionId).SendAsync("Messages", chat);
        return Ok();
    }
}
