namespace Order.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController(IOrderService orderService, IEventBus eventBus) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;
    private readonly IEventBus _eventBus = eventBus;

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

        var createdOrder = await _orderService.CreateOrderAsync(createOrderDto, userId);
        if (createdOrder == null)
        {
            return BadRequest("Failed to create order");
        }

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = createdOrder.Id,
            UserId = userId,
            Status = createdOrder.Status,
            TotalPrice = createdOrder.TotalPrice,
            Items = createdOrder.Items
        };
        _eventBus.Publish(orderCreatedEvent);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }
        if(order.UserId != userId) return Forbid();
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(string userId)
    {
        var userNameId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        if (orders == null)
        {
            return NotFound();
        }
        if(userRole != "Admin" && orders.Any(o => o.UserId != userNameId)) return Forbid();
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] string status)
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