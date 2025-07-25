using Shared.Event.Enums;

namespace Shared.Event;

public class OrderCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid PaymentId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public List<SharedOrderItem> Items { get; set; } = new List<SharedOrderItem>();
}

public class SharedOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public SharedOrderItem(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}

