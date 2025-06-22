using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Seeders;
using Identity.API.Services;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Identity.API.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
    var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "identitydb";
    var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
    var connectionString = $"server={host};port={port};database={database};user={user};password={password};";
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "localhost",
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "your-secret-key-here-change-this-in-production"))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<IGenericService<ApplicationUser>, ApplicationUserService>();
builder.Services.AddScoped<IGenericService<UserAddress>, UserAddressService>();
builder.Services.AddScoped<IGenericService<UserProfile>, UserProfileService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    SeedRole.SeedRolesAsync(services).Wait();
    Console.WriteLine("Roles seeded");
    SeedUser.SeedUsersAsync(services).Wait();
    Console.WriteLine("Users seeded");
    SeedUserRole.SeedUserRolesAsync(services).Wait();
    Console.WriteLine("User roles seeded");
}

app.Run();