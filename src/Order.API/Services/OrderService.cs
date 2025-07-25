namespace Order.API.Services;

public class OrderService(IProductService productService, ILogger<OrderService> logger, ApplicationDbContext context) : IOrderService
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<OrderService> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task<CreateOrderResult> CreateOrderAsync(CreateOrderDto createOrderDto, Guid userId)
    {
        var order = new CustomerOrder
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>(),
            PaymentId = Guid.NewGuid(),
            PaymentStatus = PaymentStatus.Pending,
            PaymentMethod = createOrderDto.PaymentMethod,
        };
        if (createOrderDto.Items == null || createOrderDto.Items.Count == 0)
        {
            _logger.LogError("Order cannot be empty");
            return CreateOrderResult.Fail("Order cannot be empty");
        }
        foreach (var item in createOrderDto.Items)
        {
            var product = await _productService.GetProductByIdAsync(item.ProductId);
            if (product == null)
            {
                _logger.LogError("Product with Id: {ProductId} not found", item.ProductId);
                return CreateOrderResult.Fail($"Product with Id: {item.ProductId} not found");
            }
            if (item.Quantity > product.StockQuantity)
            {
                _logger.LogError("Not enough quantity for ProductId: {ProductId}", item.ProductId);
                return CreateOrderResult.Fail($"Not enough quantity for ProductId: {item.ProductId}");
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
        _logger.LogInformation("Payment Info created with Id: {PaymentInfoId} for OrderId: {OrderId}", order.PaymentId, order.Id);
        return CreateOrderResult.Ok(order);
    }

    public async Task<IEnumerable<CustomerOrder>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<CustomerOrder> GetOrderByIdAsync(Guid orderId)
    {
        return await _context.Orders.FindAsync(orderId);
    }

    public async Task<IEnumerable<CustomerOrder>> GetOrdersByUserIdAsync(Guid userId)
    {
        return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
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