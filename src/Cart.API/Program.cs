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
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "localhost",
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "your-secret-key-here-change-this-in-production"))
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

// Register services
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(
        Environment.GetEnvironmentVariable("REDIS_HOST")+":"+
        Environment.GetEnvironmentVariable("REDIS_PORT")+",password="+
        Environment.GetEnvironmentVariable("REDIS_PASSWORD"));
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddScoped<ILogger<CartService>, Logger<CartService>>();

// Register controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

