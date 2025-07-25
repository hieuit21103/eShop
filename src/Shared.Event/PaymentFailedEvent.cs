namespace Shared.Event;

public class PaymentFailedEvent
{
    public Guid OrderId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Reason { get; set; }

    public PaymentFailedEvent(Guid orderId, DateTime paymentDate, string reason)
    {
        OrderId = orderId;
        PaymentDate = paymentDate;
        Reason = reason;
    }
}