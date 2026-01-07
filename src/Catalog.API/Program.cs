var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
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

// Configure Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configurationOptions = ConfigurationOptions.Parse(builder.Configuration["Redis:Endpoint"] ?? throw new InvalidOperationException("Redis endpoint is not configured."));
    configurationOptions.User = builder.Configuration["Redis:User"] ?? "default";
    configurationOptions.Password = builder.Configuration["Redis:Password"] ?? throw new InvalidOperationException("Redis password is not configured.");
    return ConnectionMultiplexer.Connect(configurationOptions);
});

// Configure DbContext
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
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<ICacheService, CacheService>();

// Register decorators
builder.Services.AddScoped<IProductService>(sp =>
    new ProductDecorator(
        sp.GetRequiredService<ProductService>(),
        sp.GetRequiredService<ICacheService>()
    )
);
builder.Services.AddScoped<IBrandService>(sp =>
    new BrandDecorator(
        sp.GetRequiredService<BrandService>(),
        sp.GetRequiredService<ICacheService>()
    )
);
builder.Services.AddScoped<ICategoryService>(sp =>
    new CategoryDecorator(
        sp.GetRequiredService<CategoryService>(),
        sp.GetRequiredService<ICacheService>()
    )
);

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

app.Run();
