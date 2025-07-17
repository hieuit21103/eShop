namespace Order.API.Services;

public interface IOrderService
{
    Task<IEnumerable<CustomerOrder>> GetAllOrdersAsync();
    Task<CustomerOrder> CreateOrderAsync(CreateOrderDto createOrderDto, string userId);
    Task<CustomerOrder> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<CustomerOrder>> GetOrdersByUserIdAsync(string userId);
    Task UpdateOrderStatusAsync(Guid orderId, string status);
    Task DeleteOrderAsync(Guid orderId);
}