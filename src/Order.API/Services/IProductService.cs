namespace Order.API.Services;

public interface IProductService
{
    Task<Product> GetProductByIdAsync(Guid productId);
}