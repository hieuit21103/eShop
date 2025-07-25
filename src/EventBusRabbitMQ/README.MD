# EventBusRabbitMQ

Thư viện Event Bus sử dụng RabbitMQ cho communication giữa các microservices.

## 📋 Mô tả

EventBusRabbitMQ cung cấp abstraction layer cho việc publish và subscribe events giữa các services trong hệ thống eShop. Sử dụng RabbitMQ làm message broker.

## 🛠️ Công nghệ

- **.NET Standard 2.1**: Tương thích đa platform
- **RabbitMQ.Client**: Client library cho RabbitMQ
- **Newtonsoft.Json**: JSON serialization
- **Microsoft.Extensions.Logging**: Logging support
- **Microsoft.Extensions.DependencyInjection**: DI support

## 🏗️ Architecture

### Interfaces

```csharp
public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : class;
    void Subscribe<T, THandler>()
        where T : class
        where THandler : class, IEventHandler<T>;
    void Unsubscribe<T, THandler>()
        where T : class
        where THandler : class, IEventHandler<T>;
}

public interface IEventHandler<in T> where T : class
{
    Task Handle(T @event);
}
```

## 🔧 Cấu hình

### appsettings.json

```json
{
  "EventBus": {
    "RabbitMQ": {
      "Host": "localhost",
      "Port": 5672,
      "Username": "guest",
      "Password": "guest",
      "VirtualHost": "/",
      "ExchangeName": "eshop_event_bus",
      "QueueName": "eshop_queue"
    }
  }
}
```

### Dependency Injection

```csharp
services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();
services.AddSingleton<IEventBus, EventBusRabbitMQ>();
services.AddTransient<IEventHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
```

## 🚀 Sử dụng

### Định nghĩa Event

```csharp
public class ProductCreatedEvent
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Event Handler

```csharp
public class ProductCreatedEventHandler : IEventHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ProductCreatedEvent @event)
    {
        _logger.LogInformation($"Product created: {@event.ProductName}");
        
        // Xử lý business logic
        await ProcessProductCreated(@event);
    }
}
```

### Publish Event

```csharp
public class ProductService
{
    private readonly IEventBus _eventBus;

    public ProductService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task CreateProductAsync(Product product)
    {
        // Lưu product vào database
        await _repository.CreateAsync(product);

        // Publish event
        var @event = new ProductCreatedEvent
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
            CreatedAt = DateTime.UtcNow
        };

        await _eventBus.PublishAsync(@event);
    }
}
```

### Subscribe Event

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure other services...
        
        services.AddTransient<ProductCreatedEventHandler>();
    }

    public void Configure(IApplicationBuilder app, IEventBus eventBus)
    {
        // Subscribe to events
        eventBus.Subscribe<ProductCreatedEvent, ProductCreatedEventHandler>();
    }
}
```

## 🔄 Event Flow

1. **Publish**: Service publish event to exchange
2. **Route**: RabbitMQ route message to appropriate queues
3. **Consume**: Subscribed services receive and process events
4. **Acknowledge**: Confirm message processing completion

## 📡 Supported Events

### Catalog Events
- `ProductCreatedEvent`
- `ProductUpdatedEvent` 
- `ProductDeletedEvent`
- `StockUpdatedEvent`

### Order Events
- `OrderCreatedEvent`
- `OrderStatusChangedEvent`
- `OrderCancelledEvent`

### Payment Events
- `PaymentCompletedEvent`
- `PaymentFailedEvent`
- `PaymentRefundedEvent`

### Cart Events
- `CartUpdatedEvent`
- `CartClearedEvent`

## 🔧 Configuration Options

### Connection Settings

```csharp
public class RabbitMQOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "eshop_event_bus";
    public int RetryCount { get; set; } = 3;
    public int ConnectionTimeout { get; set; } = 30;
}
```

### Queue Settings

```csharp
public class QueueOptions
{
    public bool Durable { get; set; } = true;
    public bool Exclusive { get; set; } = false;
    public bool AutoDelete { get; set; } = false;
    public Dictionary<string, object> Arguments { get; set; }
}
```

## 🔒 Error Handling

- **Dead Letter Queue**: Messages lỗi được chuyển vào DLQ
- **Retry Mechanism**: Tự động retry với exponential backoff
- **Circuit Breaker**: Tạm dừng khi có quá nhiều lỗi
- **Logging**: Chi tiết log cho debugging

## 📊 Monitoring

- Connection health monitoring
- Message throughput metrics
- Error rate tracking
- Queue depth monitoring

## 🧪 Testing

### Unit Tests

```csharp
[Test]
public async Task PublishAsync_ValidEvent_PublishesSuccessfully()
{
    // Arrange
    var mockConnection = new Mock<IRabbitMQPersistentConnection>();
    var eventBus = new EventBusRabbitMQ(mockConnection.Object);

    // Act & Assert
    await eventBus.PublishAsync(new ProductCreatedEvent());
}
```

### Integration Tests

```csharp
[Test]
public async Task EventBus_PublishSubscribe_Integration()
{
    // Test end-to-end event flow
}
```

## 🔗 Dependencies

- RabbitMQ server
- Shared.Event library (for event definitions)