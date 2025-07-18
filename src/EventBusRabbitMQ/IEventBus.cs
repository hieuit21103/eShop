namespace EventBusRabbitMQ;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
    Task SubscribeAsync<TEvent, THandler>()
        where TEvent : IntegrationEvent
        where THandler : IIntegrationEventHandler<TEvent>;
}