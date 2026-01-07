namespace Catalog.API.Application.Interfaces;

public interface IBrandService
{
    Task<BrandResponse> GetByIdAsync(Guid id);
    Task<BrandResponse> GetBySlugAsync(string slug);
    Task<PagedResult<BrandResponse>> GetAllAsync(BrandFilterParams filterParams);
    Task<BrandResponse> CreateAsync(BrandRequest request);
    Task<BrandResponse> UpdateAsync(BrandUpdateRequest request);
    Task DeleteAsync(Guid id);
}
