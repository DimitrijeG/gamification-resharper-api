using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReSharperGamificationApi.Hubs;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(GamificationProfile));
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddSignalR().AddHubOptions<LeaderboardHub>(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddDbContext<GamificationContext>(opt =>
    opt.UseSqlite("Data Source=gamification.sqlite"));

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://oauth.account.jetbrains.com/";
        options.Audience = "ide";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = "https://oauth.account.jetbrains.com/",
            ValidAudience = "ide"
        };
        options.RequireHttpsMetadata = true;
        options.MetadataAddress = "https://oauth.account.jetbrains.com/.well-known/openid-configuration";
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError("Auth failed: {Message}", context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

// Setup API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<LeaderboardHub>("/leaderboardHub");

app.MapRazorPages();
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();