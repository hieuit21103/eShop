namespace Order.API.Services;

public interface IOrderService
{
    Task<PagedResult<CustomerOrder>> GetAllOrdersAsync(int page = 1, int pageSize = 10, OrderStatus status = OrderStatus.Pending);
    Task<CreateOrderResult> CreateOrderAsync(CreateOrderDto createOrderDto, Guid userId);
    Task<CustomerOrder> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<CustomerOrder>> GetOrdersByUserIdAsync(Guid userId);
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task DeleteOrderAsync(Guid orderId);
}