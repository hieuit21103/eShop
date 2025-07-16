namespace Cart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController(ICartService cartService, IConnectionMultiplexer redis) : ControllerBase
{
    private readonly ICartService _cartService = cartService;
    private readonly IDatabase _database = redis.GetDatabase();

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        var cart = await cartService.GetCartAsync(userId);
        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart([FromBody] CustomerCart cart)
    {
        var updatedCart = await cartService.UpdateCartAsync(cart);
        return Ok(updatedCart);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteCart(string userId)
    {
        var result = await cartService.DeleteCartAsync(userId);
        return result ? NoContent() : NotFound();
    }
}