namespace Order.API.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    Task UpdateOrderStatusAsync(Guid orderId, string status);
    Task DeleteOrderAsync(Guid orderId);
}