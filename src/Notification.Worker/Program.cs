using MassTransit;
using Notification.Worker.Consumers;
using Notification.Worker.Models;

var builder = Host.CreateApplicationBuilder(args);

// Configure SMTP Settings
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserRegisteredConsumer>();
    x.AddConsumer<UserPasswordResetRequestedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? throw new InvalidOperationException("RabbitMQ:Host is not configured"), "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? throw new InvalidOperationException("RabbitMQ:Username is not configured"));
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? throw new InvalidOperationException("RabbitMQ:Password is not configured"));
        });

        cfg.ReceiveEndpoint("user-registered-queue", e =>
        {
            e.ConfigureConsumer<UserRegisteredConsumer>(context);
            
            // Retry Policy
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });

        cfg.ReceiveEndpoint("user-password-reset-queue", e =>
        {
            e.ConfigureConsumer<UserPasswordResetRequestedConsumer>(context);
            
            // Retry Policy
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });
    });
});

var host = builder.Build();
host.Run();
