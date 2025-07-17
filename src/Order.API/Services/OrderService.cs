namespace Order.API.Services;

public class OrderService(ILogger<OrderService> logger, ApplicationDbContext context) : IOrderService<Order>(logger, context)
{
    private readonly ILogger<OrderService> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto, string userId)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Order created with Id: {OrderId}", order.Id);
        return order;
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        return await _context.Orders.FindAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, string status)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Order status updated for OrderId: {OrderId} to Status: {Status}", orderId, status);
        }
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Order deleted with Id: {OrderId}", orderId);
        }
    }
}