# Shared.Event

Thư viện chia sẻ định nghĩa events giữa các microservices trong hệ thống eShop.

## 📋 Mô tả

Shared.Event chứa tất cả định nghĩa events, interfaces và base classes được sử dụng chung giữa các services để đảm bảo tính nhất quán trong communication.

## 🛠️ Công nghệ

- **.NET Standard 2.1**: Tương thích đa platform
- **System.Text.Json**: JSON serialization attributes
- **FluentValidation**: Event validation (optional)

## 🏗️ Structure

```
Shared.Event/
├── Base/
│   ├── IEvent.cs
│   ├── BaseEvent.cs
│   └── IEventHandler.cs
├── Catalog/
│   ├── ProductCreatedEvent.cs
│   ├── ProductUpdatedEvent.cs
│   ├── ProductDeletedEvent.cs
│   └── StockUpdatedEvent.cs
├── Order/
│   ├── OrderCreatedEvent.cs
│   ├── OrderStatusChangedEvent.cs
│   └── OrderCancelledEvent.cs
├── Payment/
│   ├── PaymentCompletedEvent.cs
│   ├── PaymentFailedEvent.cs
│   └── PaymentRefundedEvent.cs
└── Cart/
    ├── CartUpdatedEvent.cs
    └── CartClearedEvent.cs
```

## 📦 Base Classes & Interfaces

### IEvent

```csharp
public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
    string EventType { get; }
}
```

### BaseEvent

```csharp
public abstract class BaseEvent : IEvent
{
    public Guid EventId { get; private set; } = Guid.NewGuid();
    public DateTime OccurredAt { get; private set; } = DateTime.UtcNow;
    public abstract string EventType { get; }
}
```

### IEventHandler

```csharp
public interface IEventHandler<in T> where T : IEvent
{
    Task Handle(T @event);
}
```

## 📡 Catalog Events

### ProductCreatedEvent

```csharp
public class ProductCreatedEvent : BaseEvent
{
    public override string EventType => "ProductCreated";
    
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public string ImageUrl { get; set; }
    public Guid CreatedBy { get; set; }
}
```

### ProductUpdatedEvent

```csharp
public class ProductUpdatedEvent : BaseEvent
{
    public override string EventType => "ProductUpdated";
    
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public string ImageUrl { get; set; }
    public Guid UpdatedBy { get; set; }
    public Dictionary<string, object> ChangedFields { get; set; }
}
```

### StockUpdatedEvent

```csharp
public class StockUpdatedEvent : BaseEvent
{
    public override string EventType => "StockUpdated";
    
    public Guid ProductId { get; set; }
    public int PreviousStock { get; set; }
    public int NewStock { get; set; }
    public string Reason { get; set; }
    public Guid UpdatedBy { get; set; }
}
```

## 📦 Order Events

### OrderCreatedEvent

```csharp
public class OrderCreatedEvent : BaseEvent
{
    public override string EventType => "OrderCreated";
    
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public List<OrderItemData> Items { get; set; }
    public AddressData ShippingAddress { get; set; }
}

public class OrderItemData
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class AddressData
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
}
```

### OrderStatusChangedEvent

```csharp
public class OrderStatusChangedEvent : BaseEvent
{
    public override string EventType => "OrderStatusChanged";
    
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string PreviousStatus { get; set; }
    public string NewStatus { get; set; }
    public string Reason { get; set; }
    public Guid ChangedBy { get; set; }
}
```

## 💳 Payment Events

### PaymentCompletedEvent

```csharp
public class PaymentCompletedEvent : BaseEvent
{
    public override string EventType => "PaymentCompleted";
    
    public Guid PaymentId { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string