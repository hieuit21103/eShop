namespace Order.API.Services;

public interface IOrderService
{
    Task<IEnumerable<CustomerOrder>> GetAllOrdersAsync();
    Task<CreateOrderResult> CreateOrderAsync(CreateOrderDto createOrderDto, Guid userId);
    Task<CustomerOrder> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<CustomerOrder>> GetOrdersByUserIdAsync(Guid userId);
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task DeleteOrderAsync(Guid orderId);
}