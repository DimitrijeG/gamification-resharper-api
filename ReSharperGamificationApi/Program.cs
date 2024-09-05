using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AchievementContext>(opt =>
    opt.UseInMemoryDatabase("Achievements"));

// Configure CORS
const string frontendCors = "FrontendGetPolicy";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
    options.AddPolicy(frontendCors, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .WithMethods("GET");
    });
});

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(frontendCors);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
