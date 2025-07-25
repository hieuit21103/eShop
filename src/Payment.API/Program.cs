var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

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

// Add ApplicationDbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var host = Environment.GetEnvironmentVariable("DB_HOST");
    var port = Environment.GetEnvironmentVariable("DB_PORT");
    var database = Environment.GetEnvironmentVariable("DB_DATABASE");
    var user = Environment.GetEnvironmentVariable("DB_USERNAME");
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var connectionString = $"server={host};port={port};database={database};user={user};password={password};";
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddHttpContextAccessor();

// Add services
builder.Services.AddScoped<IPaymentService<PaymentService>, PaymentService>();
builder.Services.AddScoped<IPaymentServiceProvider, VnPayServiceProvider>();

builder.Services.AddControllers();
// Add EventBusRabbitMQ
builder.Services.AddEventBus();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();

// app.UseHttpsRedirection();

app.Run();

