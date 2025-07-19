namespace EventBusRabbitMQ;

public class EventBusRabbitMQ : IEventBus, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChannel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<EventBusRabbitMQ> _logger;
    private readonly List<AsyncEventingBasicConsumer> _consumers = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _disposed = false;

    private EventBusRabbitMQ(IServiceProvider serviceProvider, IConnection connection, IChannel channel, ILogger<EventBusRabbitMQ> logger)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _channel = channel;
        _logger = logger;
    }

    public static async Task<EventBusRabbitMQ> CreateAsync(IServiceProvider serviceProvider)
    {

        var factory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
            Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "eshop_eventbus",
            type: "direct",
            durable: true);

        var logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>();
        return new EventBusRabbitMQ(serviceProvider, connection, channel, logger);
    }

    public async Task PublishAsync(IntegrationEvent @event)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(EventBusRabbitMQ));

        try
        {
            var eventName = @event.GetType().Name;
            var json = JsonSerializer.Serialize(@event, @event.GetType());
            var body = Encoding.UTF8.GetBytes(json);

            _logger.LogInformation("Publishing event {EventName} with ID {EventId}", eventName, @event.Id);

            await _channel.BasicPublishAsync(
                exchange: "eshop_eventbus",
                routingKey: eventName,
                body: body);

            _logger.LogInformation("Published event {EventName} with ID {EventId}", eventName, @event.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType}", @event.GetType().Name);
            throw;
        }
    }

    public async Task SubscribeAsync<TEvent, THandler>()
        where TEvent : IntegrationEvent
        where THandler : IIntegrationEventHandler<TEvent>
    {
        if (_disposed) throw new ObjectDisposedException(nameof(EventBusRabbitMQ));

        await _semaphore.WaitAsync();
        try
        {
            var eventName = typeof(TEvent).Name;

            await _channel.QueueDeclareAsync(
                queue: eventName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            await _channel.QueueBindAsync(
                queue: eventName,
                exchange: "eshop_eventbus",
                routingKey: eventName);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var deliveryTag = ea.DeliveryTag;
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("[EventBusRabbitMQ] Raw JSON received for event {EventName}: {Json}", eventName, json);
                    var @event = JsonSerializer.Deserialize<TEvent>(json);

                    if (@event == null)
                    {
                        _logger.LogWarning("Failed to deserialize event {EventName}", eventName);
                        await _channel.BasicNackAsync(deliveryTag, false, false);
                        return;
                    }

                    using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<THandler>();
                    await handler.Handle(@event);

                    await _channel.BasicAckAsync(deliveryTag, false);
                    _logger.LogInformation("Successfully processed event {EventName} with ID {EventId}", eventName, @event.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing event {EventName}", eventName);
                    await _channel.BasicNackAsync(deliveryTag, false, true); // Requeue on error
                }
            };

            await _channel.BasicConsumeAsync(
                queue: eventName,
                autoAck: false, // Manual acknowledgment
                consumer: consumer);

            _consumers.Add(consumer);
            _logger.LogInformation("Subscribed to event {EventName} with handler {HandlerName}", eventName, typeof(THandler).Name);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        await _semaphore.WaitAsync();
        try
        {
            _disposed = true;

            // Dispose consumers
            foreach (var consumer in _consumers)
            {
                // Consumers are automatically disposed when channel is disposed
            }
            _consumers.Clear();

            if (_channel != null)
                await _channel.DisposeAsync();

            if (_connection != null)
                await _connection.DisposeAsync();
        }
        finally
        {
            _semaphore.Release();
            _semaphore.Dispose();
        }
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }
}