namespace Order.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService<Order> _orderService;
    private readonly string _jwtToken;

    public OrderController(IOrderService<Order> orderService)
    {
        _orderService = orderService;
        _jwtToken = Request.Cookies["JWT"];
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var userId = _jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID is required");
        }
        if (order == null)
        {
            return BadRequest("Order cannot be null");
        }

        var createdOrder = await _orderService.CreateOrderAsync(createOrderDto, userId);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(string userId)
    {
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] string status)
    {
        await _orderService.UpdateOrderStatusAsync(orderId, status);
        return NoContent();
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        await _orderService.DeleteOrderAsync(orderId);
        return NoContent();
    }
}