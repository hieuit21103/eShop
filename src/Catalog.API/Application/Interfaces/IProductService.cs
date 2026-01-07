namespace Catalog.API.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> GetByIdAsync(Guid id);
    Task<ProductResponse> GetBySlugAsync(string slug);
    Task<PagedResult<ProductResponse>> GetAllAsync(ProductFilterParams filterParams);
    Task<ProductResponse> CreateAsync(ProductRequest request);
    Task<ProductResponse> UpdateAsync(ProductUpdateRequest request);
    Task DeleteAsync(Guid id);
}
