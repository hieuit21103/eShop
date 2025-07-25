var builder = WebApplication.CreateBuilder(args);

// Add EventBusRabbitMQ
builder.Services.AddEventBus();

// Add OpenAPI support (only once)
builder.Services.AddOpenApi();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:5001", "http://localhost:3001")
                  .AllowCredentials()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add HttpContextAccessor for dependency injection
builder.Services.AddHttpContextAccessor();

// Add ApplicationDbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
    var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "orderdb";
    var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "root";
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
    
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
    var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new InvalidOperationException("JWT_KEY environment variable is required");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromMinutes(5) // Add some clock skew tolerance
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Check for JWT in cookies first, then Authorization header
            if (context.Request.Cookies.TryGetValue("JWT", out var cookieToken))
            {
                context.Token = cookieToken;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            // Log authentication failures in development
            if (builder.Environment.IsDevelopment())
            {
                Console.WriteLine($"JWT Authentication failed: {context.Exception?.Message}");
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
builder.Services.AddScoped<IOrderService, OrderService>();

// Add HttpClient for ProductService with error handling
builder.Services.AddHttpClient<IProductService, HttpProductService>(client =>
{
    var catalogApiUrl = Environment.GetEnvironmentVariable("CATALOG_API_URL");
    if (string.IsNullOrEmpty(catalogApiUrl))
    {
        throw new InvalidOperationException("CATALOG_API_URL environment variable is required");
    }
    client.BaseAddress = new Uri(catalogApiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Add HttpClient for PaymentService with error handling
builder.Services.AddHttpClient<IPaymentService, HttpPaymentService>(client =>
{
    var paymentApiUrl = Environment.GetEnvironmentVariable("PAYMENT_API_URL");
    if (string.IsNullOrEmpty(paymentApiUrl))
    {
        throw new InvalidOperationException("PAYMENT_API_URL environment variable is required");
    }
    client.BaseAddress = new Uri(paymentApiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

// Database migration with error handling
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
        Console.WriteLine("Database migration completed successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database migration failed: {ex.Message}");
    // In production, you might want to exit here or handle differently
}

// Configure middleware pipeline
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Only redirect to HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();