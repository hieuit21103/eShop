namespace Shared.Event;

public class OrderCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public string UserId { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> Items { get; set; }
}
