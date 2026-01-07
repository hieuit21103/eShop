namespace Catalog.API.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponse> GetByIdAsync(Guid id);
    Task<CategoryResponse> GetBySlugAsync(string slug);
    Task<PagedResult<CategoryResponse>> GetAllAsync(CategoryFilterParams filterParams);
    Task<CategoryResponse> CreateAsync(CategoryRequest request);
    Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest request);
    Task DeleteAsync(Guid id);
}
