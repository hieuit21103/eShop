namespace Order.API.Models.DTOs;

public class CreateOrderDto
{
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();

    public PaymentMethod PaymentMethod { get; set; } = 0;
}