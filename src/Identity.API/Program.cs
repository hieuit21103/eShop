using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Identity API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var host = builder.Configuration["ConnectionStrings:Host"] ?? throw new InvalidOperationException("Database host is not configured.");
    var port = builder.Configuration["ConnectionStrings:Port"] ?? throw new InvalidOperationException("Database port is not configured.");
    var database = builder.Configuration["ConnectionStrings:Database"] ?? throw new InvalidOperationException("Database name is not configured.");
    var user = builder.Configuration["ConnectionStrings:Username"] ?? throw new InvalidOperationException("Database username is not configured.");
    var password = builder.Configuration["ConnectionStrings:Password"] ?? throw new InvalidOperationException("Database password is not configured.");
    var connectionString = $"host={host};port={port};database={database};username={user};password={password};";
    options.UseNpgsql(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option => option.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "localhost",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? "your-secret-key-here-change-this-in-production"))
    };
});


// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

// Register controllers
builder.Services.AddControllers();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

// Register GRPC services
builder.Services.AddGrpcClient<FileStorage.Protos.FileStorageService.FileStorageServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["FileStorage:GrpcUrl"] ?? throw new InvalidOperationException("FileStorage gRPC URL is not configured"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    await SeedRole.SeedRolesAsync(services);
    Console.WriteLine("Roles seeded");
    await SeedUser.SeedUsersAsync(services);
    Console.WriteLine("Users seeded");
    await SeedUserRole.SeedUserRolesAsync(services);
    Console.WriteLine("User roles seeded");
}

app.Run();