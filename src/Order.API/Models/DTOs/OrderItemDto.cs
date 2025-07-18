namespace Order.API.Models.DTOs;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
