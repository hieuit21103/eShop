namespace Cart.API.Services;

public class CartService(ILogger<CartService> logger, IConnectionMultiplexer redis)
    : ICartService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<CustomerCart> GetCartAsync(string userId)
    {
        var cartData = await _database.StringGetAsync(userId);
        if (cartData.IsNullOrEmpty)
        {
            return await CreateCartAsync(userId);
        }

        return JsonSerializer.Deserialize<CustomerCart>(cartData);
    }

    public async Task<CustomerCart> UpdateCartAsync(CustomerCart cart)
    {
        var cartData = JsonSerializer.Serialize(cart);
        await _database.StringSetAsync(cart.UserId, cartData);
        return cart;
    }

    public async Task<bool> DeleteCartAsync(string userId)
    {
        return await _database.KeyDeleteAsync(userId);
    }

    public async Task<CustomerCart> CreateCartAsync(string userId)
    {
        var cart = new CustomerCart(userId);
        await UpdateCartAsync(cart);
        return cart;
    }
}
