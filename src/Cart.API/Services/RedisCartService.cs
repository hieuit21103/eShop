namespace Cart.API.Services;

public class RedisCartService(ILogger<RedisCartService> logger, IConnectionMultiplexer redis)
    : ICartService
{
    private readonly IDatabase _database = redis.GetDatabase();
    private readonly ILogger<RedisCartService> _logger;

    public async Task<CustomerCart> GetCartAsync(string userId)
    {
        var cartData = await _database.StringGetAsync(userId);
        if (cartData.IsNullOrEmpty)
        {
            return new CustomerCart(userId);
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
}