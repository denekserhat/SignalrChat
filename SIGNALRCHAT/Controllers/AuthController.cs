using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGNALRCHAT.Context;
using SIGNALRCHAT.Dtos;
using SIGNALRCHAT.Models;

namespace SIGNALRCHAT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController(ApplicationDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
        {
            bool isNameExist = await context.Users.AnyAsync(x => x.Name == request.Name, cancellationToken);
            if (isNameExist)
            {
                return BadRequest("Name already exist");
            }

            if (request.Avatar.Length == 0)
            {
                return BadRequest("Avatar is required");
            }

            int avatarId = await UploadFile(request.Avatar, cancellationToken);

            User user = new User
            {
                Name = request.Name,
                FileId = avatarId
            };

            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (user == null)
            {
                return NotFound("User not found");
            }
            user.Status = "Online";
            await context.SaveChangesAsync(cancellationToken);
            return Ok(user);
        }


        public async Task<int> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);
            var fileBytes = ms.ToArray();

            var newFile = new Models.File
            {
                FileName = file.FileName,
                Content = fileBytes,
                ContentType = file.ContentType
            };
            context.Files.Add(newFile);
            await context.SaveChangesAsync();

            return newFile.Id;
        }

        public void GetFile()
        {
        }
    }
}
