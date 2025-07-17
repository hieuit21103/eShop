namespace Order.API.Models;
public class CustomerOrder
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}