namespace Shared.Event;

public class OrderCanceledEvent
{
    public Guid OrderId { get; set; }
    public DateTime CanceledAt { get; set; }

    public OrderCanceledEvent(Guid orderId, DateTime canceledAt)
    {
        OrderId = orderId;
        CanceledAt = canceledAt;
    }
}