using SIGNALRCHAT.Context;
using Microsoft.EntityFrameworkCore;
using SIGNALRCHAT.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(
    builder.Configuration.GetConnectionString("PostgreSql")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat-hub");
app.Run();
