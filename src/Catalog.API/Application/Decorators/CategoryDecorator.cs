namespace Catalog.API.Application.Decorators;

public class CategoryDecorator : ICategoryService
{
    private readonly ICategoryService _categoryService;
    private readonly ICacheService _cacheService;
    public CategoryDecorator(ICategoryService categoryService, ICacheService cacheService)
    {
        _categoryService = categoryService;
        _cacheService = cacheService;
    }

    public async Task<CategoryResponse> GetByIdAsync(Guid id)
    {
        var cacheKey = $"Category:id:{id}";
        var cachedCategory = await _cacheService.GetOrCreateAsync(cacheKey,
            () => _categoryService.GetByIdAsync(id), TimeSpan.FromDays(1));
        return cachedCategory;
    }

    public async Task<CategoryResponse> GetBySlugAsync(string slug)
    {
        var cacheKey = $"Category:slug:{slug}";
        var cachedCategory = await _cacheService.GetOrCreateAsync(cacheKey,
            () => _categoryService.GetBySlugAsync(slug), TimeSpan.FromDays(1));
        return cachedCategory;
    }

    public async Task<PagedResult<CategoryResponse>> GetAllAsync(CategoryFilterParams filterParams)
    {
        return await _categoryService.GetAllAsync(filterParams);
    }

    public async Task<CategoryResponse> CreateAsync(CategoryRequest request)
    {
        return await _categoryService.CreateAsync(request);
    }

    public async Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest request)
    {
        var oldCategory = await _categoryService.GetByIdAsync(request.Id);
        var category = await _categoryService.UpdateAsync(request);
        await _cacheService.RemoveAsync($"Category:id:{oldCategory.Id}");
        if (oldCategory != null)
            await _cacheService.RemoveAsync($"Category:slug:{oldCategory.Slug}");
        if (category.Slug != oldCategory?.Slug)
            await _cacheService.RemoveAsync($"Category:slug:{category.Slug}");
        return category;
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        await _categoryService.DeleteAsync(id);
        await _cacheService.RemoveAsync($"Category:slug:{category.Slug}");
        await _cacheService.RemoveAsync($"Category:id:{category.Id}");
    }
}