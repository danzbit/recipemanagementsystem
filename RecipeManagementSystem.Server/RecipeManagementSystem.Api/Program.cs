using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using RecipeManagementSystem.Application.Configurations;
using RecipeManagementSystem.Infrastructure.Configurations;
using RecipeManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipeManagementSystem.Application.Common.Handlers;
using RecipeManagementSystem.Application.Hubs;
using RecipeManagementSystem.Application.Kafka;
using RecipeManagementSystem.Application.Settings;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Server.Middlewares;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection(KafkaOptions.Kafka));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
Validator.ValidateObject(jwtSettings, new ValidationContext(jwtSettings), validateAllProperties: true);

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

var kafkaOptions = builder.Configuration.GetSection(KafkaOptions.Kafka).Get<KafkaOptions>();

if (kafkaOptions is not null)
{
    builder.Services.AddKafkaProducer();

    builder.Services.AddKafkaConsumer<ResendInviteExecutionMessage, ResendInviteExecutionHandler>(
        bootstrapServer: kafkaOptions.BootstrapServer,
        topic: KafkaTopics.ResendInvitesExecution,
        groupId: kafkaOptions.GroupIds.ManagementRecipeInvites);
    
    builder.Services.AddKafkaConsumer<AutoExpireInviteExecutionMessage, AutoExpireInviteExecutionHandler>(
        bootstrapServer: kafkaOptions.BootstrapServer,
        topic: KafkaTopics.ExpiredInviteExecution,
        groupId: kafkaOptions.GroupIds.ManagementRecipeInvites);
}

builder.Services.AddMemoryCache();
builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Debug.Write(ex, "An error occurred while applying migrations.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.MapHub<PdfStatusHub>("/hubs/pdf-status");
app.MapHub<InviteHub>("/hubs/invite");
app.MapHub<RecipeHub>("/hubs/recipe");

app.Run();
