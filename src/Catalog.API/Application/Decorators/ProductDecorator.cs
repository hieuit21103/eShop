namespace Catalog.API.Application.Decorators;

public class ProductDecorator : IProductService
{
    private readonly IProductService _productService;
    private readonly ICacheService _cacheService;

    public ProductDecorator(IProductService productService, ICacheService cacheService)
    {
        _productService = productService;
        _cacheService = cacheService;
    }

    public async Task<ProductResponse> GetByIdAsync(Guid id)
    {
        var cacheKey = $"product:id:{id}";
        var cachedProduct = await _cacheService.GetOrCreateAsync(cacheKey,
            () => _productService.GetByIdAsync(id), TimeSpan.FromHours(4));
        return cachedProduct;

    }

    public async Task<ProductResponse> GetBySlugAsync(string slug)
    {
        var cacheKey = $"product:slug:{slug}";
        var cachedProduct = await _cacheService.GetOrCreateAsync(cacheKey,
            () => _productService.GetBySlugAsync(slug), TimeSpan.FromHours(4));
        return cachedProduct;
    }

    public async Task<PagedResult<ProductResponse>> GetAllAsync(ProductFilterParams filter)
    {
        bool isCacheable = filter.PageNumber == 1 && string.IsNullOrEmpty(filter.Search);

        if (!isCacheable)
        {
            return await _productService.GetAllAsync(filter);
        }

        var cacheKey = $"product:all:cat{filter.CategoryId}:brand{filter.BrandId}:sort{filter.SortBy}";

        var cachedData = await _cacheService.GetOrCreateAsync(cacheKey,
            () => _productService.GetAllAsync(filter), TimeSpan.FromMinutes(10));

        return cachedData;
    }

    public async Task<ProductResponse> CreateAsync(ProductRequest request)
    {
        return await _productService.CreateAsync(request);
    }

    public async Task<ProductResponse> UpdateAsync(ProductUpdateRequest request)
    {
        var oldProduct = await _productService.GetByIdAsync(request.Id);
        var newProduct = await _productService.UpdateAsync(request);
        await _cacheService.RemoveAsync($"product:id:{request.Id}");
        if (oldProduct != null)
            await _cacheService.RemoveAsync($"product:slug:{oldProduct.Slug}");
        if (newProduct.Slug != oldProduct?.Slug)
            await _cacheService.RemoveAsync($"product:slug:{newProduct.Slug}");
        return newProduct;
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        await _productService.DeleteAsync(id);
        if (product != null)
        {
            await _cacheService.RemoveAsync($"product:id:{id}");
            await _cacheService.RemoveAsync($"product:slug:{product.Slug}");
        }
    }
}