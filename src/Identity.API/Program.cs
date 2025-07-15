using eShop.Identity.API.Extensions;
using Identity.API.Data;
using Identity.API.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
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