namespace Order.API.Services;

public class OrderService(IProductService productService, ILogger<OrderService> logger, ApplicationDbContext context) : IOrderService
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<OrderService> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task<CustomerOrder> CreateOrderAsync(CreateOrderDto createOrderDto, string userId)
    {
        var order = new CustomerOrder
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = "Pending",
            Items = new List<OrderItem>()
        };
        if (createOrderDto.Items == null || createOrderDto.Items.Count == 0)
        {
            _logger.LogError("Order cannot be empty");
            return null;
        }
        foreach (var item in createOrderDto.Items)
        {
            var product = await _productService.GetProductByIdAsync(item.ProductId);
            if (product == null)
            {
                _logger.LogError("Product with Id: {ProductId} not found", item.ProductId);
                return null;
            }
            if (item.Quantity > product.StockQuantity)
            {
                _logger.LogError("Not enough quantity for ProductId: {ProductId}", item.ProductId);
                return null;
            }
            order.Items.Add(new OrderItem(
                id: Guid.NewGuid(),
                orderId: order.Id,
                productId: item.ProductId,
                name: product.Name,
                unitPrice: product.UnitPrice,
                quantity: item.Quantity
            ));
        }
        order.TotalPrice = order.Items.Sum(item => item.Quantity * item.UnitPrice);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Order created with Id: {OrderId}", order.Id);
        return order;
    }

    public async Task<IEnumerable<CustomerOrder>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<CustomerOrder> GetOrderByIdAsync(Guid orderId)
    {
        return await _context.Orders.FindAsync(orderId);
    }

    public async Task<IEnumerable<CustomerOrder>> GetOrdersByUserIdAsync(string userId)
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