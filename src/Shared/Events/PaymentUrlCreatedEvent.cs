namespace Shared.Event;

public class PaymentUrlCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string PaymentUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public PaymentUrlCreatedEvent(Guid orderId, Guid userId, string paymentUrl)
    {
        OrderId = orderId;
        UserId = userId;
        PaymentUrl = paymentUrl;
        CreatedAt = DateTime.UtcNow.AddHours(7);
    }
}