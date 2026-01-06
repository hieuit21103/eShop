namespace Shared.Event;

public class PaymentSucceedEvent
{
    public Guid OrderId { get; set; }
    public DateTime PaymentDate { get; set; }

    public PaymentSucceedEvent(Guid orderId, DateTime paymentDate)
    {
        OrderId = orderId;
        PaymentDate = paymentDate;
    }
}