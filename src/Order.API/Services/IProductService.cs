namespace Order.API.Services;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(Guid productId);
}