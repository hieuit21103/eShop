namespace Shared.Event;

public class OrderCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public string UserId { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
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

