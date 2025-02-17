using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGNALRCHAT.Context;

namespace SIGNALRCHAT.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class ChatsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ChatsController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> GetChats(Guid userId, CancellationToken cancellationToken)
    {
        var chats = await _context.Chats
            .Where(x => x.UserId == userId || x.ToUserId == userId)
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);
        return Ok(chats);
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage(Chat chat, CancellationToken cancellationToken)
    {
        chat.Date = DateTime.Now;
        await _context.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
