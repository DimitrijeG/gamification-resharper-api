using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AchievementContext>(opt =>
    opt.UseInMemoryDatabase("AchievementList"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
