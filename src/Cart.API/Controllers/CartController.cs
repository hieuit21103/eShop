using Microsoft.IdentityModel.JsonWebTokens;

namespace Cart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController(ICartService cartService, IConnectionMultiplexer redis, ILogger<CartController> logger) : ControllerBase
{
    private readonly ICartService _cartService = cartService;
    private readonly IDatabase _database = redis.GetDatabase();
    private readonly ILogger<CartController> _logger = logger;

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        var userTokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userTokenRole = User.FindFirst(ClaimTypes.Role)?.Value;
        _logger.LogInformation("UserId: {UserId}, UserTokenId: {UserTokenId}, UserTokenRole: {UserTokenRole}", userId, userTokenId, userTokenRole);
        if (userTokenRole != "Admin" && userId != userTokenId) return Forbid();
        var cart = await cartService.GetCartAsync(userId);
        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart([FromBody] CustomerCart cart)
    {
        var userTokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (cart.UserId != userTokenId) return Forbid();
        var updatedCart = await cartService.UpdateCartAsync(cart);
        return Ok(updatedCart);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteCart(string userId)
    {    
        var userTokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await cartService.DeleteCartAsync(userId);
        return result ? NoContent() : NotFound();
    }
}