namespace Cart.API.Grpc;

public class CartService : ICartService
{
    private readonly ICartService _cartService;

    public CartService(ICartService cartService)
    {
        _cartService = cartService;
    }

    public Task<CustomerCart> GetCartAsync(string userId)
    {
        return _cartService.GetCartAsync(userId);
    }

    public Task<CustomerCart> UpdateCartAsync(CustomerCart cart)
    {
        return _cartService.UpdateCartAsync(cart);
    }

    public Task<bool> DeleteCartAsync(string userId)
    {
        return _cartService.DeleteCartAsync(userId);
    }
}