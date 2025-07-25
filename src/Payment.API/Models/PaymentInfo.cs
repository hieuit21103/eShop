namespace Payment.API.Models;

public class PaymentInfo
{
    public Guid Id { get; set; }
    public string? TransactionId { get; set; }
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public string PaymentUrl { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = 0;
    public PaymentStatus Status { get; set; } = 0;
}