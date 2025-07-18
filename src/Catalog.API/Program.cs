var builder = WebApplication.CreateBuilder(args);

//Load environment variables from .env file
DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("localhost", "https://localhost:5001")
                  .AllowCredentials()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add OpenAPI support
builder.Services.AddOpenApi();

// Add ApplicationDbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var host = Environment.GetEnvironmentVariable("DB_HOST");
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
    var database = Environment.GetEnvironmentVariable("DB_DATABASE");
    var user = Environment.GetEnvironmentVariable("DB_USERNAME");
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var connectionString = $"server={host};port={port};database={database};user={user};password={password};";
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

// Add JWT authentication
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
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("JWT", out var cookieToken))
            {
                context.Token = cookieToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Add custom Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User", "Admin"));
});

// Add services
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IIntegrationEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();

builder.Services.AddControllers();

// Add EventBusRabbitMQ
builder.Services.AddEventBus();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Kiểm tra tất cả registered services
    var allServices = builder.Services.Select(x => x.ServiceType.Name).ToList();
    Console.WriteLine($"All registered services: {string.Join(", ", allServices)}");
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

}

// Use CORS policy
app.UseCors("AllowAll");

// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.Run();
