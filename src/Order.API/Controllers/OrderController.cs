namespace Order.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController(IOrderService orderService, IPaymentService paymentService, IEventBus eventBus, ILogger<OrderController> logger) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;
    private readonly IPaymentService _paymentService = paymentService;
    private readonly IEventBus _eventBus = eventBus;
    private readonly ILogger<OrderController> _logger = logger;

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID is required");
        }
        if (createOrderDto == null)
        {
            return BadRequest("Order cannot be null");
        }

        var createdOrder = await _orderService.CreateOrderAsync(createOrderDto, Guid.Parse(userId));
        if (!createdOrder.Success)
        {
            return BadRequest(createdOrder.Error);
        }

        if (createdOrder.Order.Items == null || !createdOrder.Order.Items.Any())
        {
            _logger.LogWarning("createdOrder.Items is null or empty when creating OrderCreatedEvent for OrderId: {OrderId}", createdOrder.Order.Id);
        }
        else
        {
            foreach (var item in createdOrder.Order.Items)
            {
                _logger.LogInformation("Order item logged with Product ID: {ProductId}, Quantity: {Quantity}", item.ProductId, item.Quantity);
            }
        }

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = createdOrder.Order.Id,
            UserId = Guid.Parse(userId),
            Status = createdOrder.Order.Status,
            TotalPrice = createdOrder.Order.TotalPrice,
            PaymentId = createdOrder.Order.PaymentId,
            PaymentStatus = createdOrder.Order.PaymentStatus,
            Items = createdOrder.Order.Items == null ? new List<SharedOrderItem>() : createdOrder.Order.Items.Select(i => new SharedOrderItem(i.ProductId, i.Quantity)).ToList()
        };
        foreach (var item in orderCreatedEvent.Items)
        {
            _logger.LogInformation("OrderCreatedEvent Item: ProductId={ProductId}, Quantity={Quantity}", item.ProductId, item.Quantity);
        }
        await _eventBus.PublishAsync(orderCreatedEvent);
        var response = await _paymentService.CreatePaymentAsync(createdOrder.Order);
        if (response == null)
        {
            _logger.LogError("Payment creation failed for OrderId: {OrderId}", createdOrder.Order.Id);
            return BadRequest("Failed to create payment for the order");
        }
        return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Order.Id }, response);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID is required");
        }
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }
        if (order.UserId != Guid.Parse(userId)) return Forbid();
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(Guid userId)
    {
        var userNameId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(userNameId))
        {
            return Unauthorized("User ID is required");
        }
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        if (orders == null)
        {
            return NotFound();
        }
        if (userRole != "Admin" && orders.Any(o => o.UserId != Guid.Parse(userNameId))) return Forbid();
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] OrderStatus status)
    {
        await _orderService.UpdateOrderStatusAsync(orderId, status);
        return NoContent();
    }

    [HttpDelete("{orderId}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        await _orderService.DeleteOrderAsync(orderId);
        return NoContent();
    }
}