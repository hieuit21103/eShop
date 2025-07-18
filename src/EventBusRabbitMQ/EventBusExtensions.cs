namespace EventBusRabbitMQ;

public static class EventBusExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus>(sp => 
        {
            try 
            {
                var eventBus = EventBusRabbitMQ.CreateAsync(sp).GetAwaiter().GetResult();
                return eventBus;
            }
            catch (Exception ex)
            {
                var logger = sp.GetService<ILogger<EventBusRabbitMQ>>();
                logger?.LogError(ex, "Failed to create EventBus");
                throw;
            }
        });
        return services;
    }
}