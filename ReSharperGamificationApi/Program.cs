using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

builder.Services.AddSignalR().AddHubOptions<LeaderboardHub>(opt =>
{
    opt.EnableDetailedErrors = true;
});

builder.Services.AddDbContext<GamificationContext>(opt => opt
    .UseLazyLoadingProxies()
    .UseSqlite(builder.Configuration.GetConnectionString("ReSharperGamificationDb")));

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure JWT Bearer Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = "https://oauth.account.jetbrains.com/";
        opt.Audience = "ide";
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = "https://oauth.account.jetbrains.com/",
            ValidAudience = "ide"
        };
        opt.RequireHttpsMetadata = true;
        opt.MetadataAddress = "https://oauth.account.jetbrains.com/.well-known/openid-configuration";
        opt.Events = new JwtBearerEvents
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
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1);
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
    opt.SwaggerDoc("v1", new OpenApiInfo{ Title = "ReSharper Gamification API", Version = "v1" }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<GamificationContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.SeedDatabase("development.json");
    }

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

app.Run();