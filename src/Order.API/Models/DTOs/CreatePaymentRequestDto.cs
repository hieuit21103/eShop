namespace Order.API.Models.DTOs;

public class CreatePaymentRequestDto
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}