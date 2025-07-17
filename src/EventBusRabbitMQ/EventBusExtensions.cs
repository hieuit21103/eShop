namespace EventBusRabbitMQ;

public static class EventBusExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus>(sp => 
            EventBusRabbitMQ.CreateAsync(sp).GetAwaiter().GetResult());
        return services;
    }

    public static void UseEventBus(this IServiceProvider provider, Action<IEventBus> config)
    {
        var bus = provider.GetRequiredService<IEventBus>();
        config(bus);
    }
}
