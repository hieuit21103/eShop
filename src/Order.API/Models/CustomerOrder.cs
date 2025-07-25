namespace Order.API.Models;

public class CustomerOrder
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = 0;
    public List<OrderItem> Items { get; set; } = new();
    public Guid PaymentId { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = 0;
    public PaymentMethod PaymentMethod { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
