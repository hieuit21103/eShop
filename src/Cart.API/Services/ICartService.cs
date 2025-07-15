namespace Cart.API.Services;

public interface ICartService
{
    Task<CustomerCart> GetCartAsync(string userId);
    Task<CustomerCart> UpdateCartAsync(CustomerCart cart);
    Task<bool> DeleteCartAsync(string userId);
}