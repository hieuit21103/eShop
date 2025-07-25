namespace Payment.API.Models;

public class PaymentResponse
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string? TransactionId { get; set; }
    public string? ResponseCode { get; set; }
    public string? Message { get; set; }
}