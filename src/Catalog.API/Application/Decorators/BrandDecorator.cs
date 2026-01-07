namespace Catalog.API.Application.Decorators;

public class BrandDecorator : IBrandService
{
    private readonly IBrandService _brandService;
    private readonly ICacheService _cacheService;
    public BrandDecorator(IBrandService brandService, ICacheService cacheService)
    {
        _brandService = brandService;
        _cacheService = cacheService;
    }

    public async Task<BrandResponse> GetByIdAsync(Guid id)
    {
        var cacheKey = $"brand:id:{id}";
        var cachedBrand = await _cacheService.GetOrCreateAsync(cacheKey,
            () => _brandService.GetByIdAsync(id), TimeSpan.FromDays(1));
        return cachedBrand;
    }

    public async Task<BrandResponse> GetBySlugAsync(string slug)
    {
        var cacheKey = $"brand:slug:{slug}";
        var cachedBrand = await _cacheService.GetOrCreateAsync(cacheKey,
            () => _brandService.GetBySlugAsync(slug), TimeSpan.FromDays(1));
        return cachedBrand;
    }

    public async Task<PagedResult<BrandResponse>> GetAllAsync(BrandFilterParams filterParams)
    {
        return await _brandService.GetAllAsync(filterParams);
    }

    public async Task<BrandResponse> CreateAsync(BrandRequest request)
    {
        return await _brandService.CreateAsync(request);
    }

    public async Task<BrandResponse> UpdateAsync(BrandUpdateRequest request)
    {
        var oldBrand = await _brandService.GetByIdAsync(request.Id);
        var brand = await _brandService.UpdateAsync(request);
        await _cacheService.RemoveAsync($"brand:id:{brand.Id}");
        if (oldBrand != null)
            await _cacheService.RemoveAsync($"brand:slug:{oldBrand.Slug}");
        if (brand.Slug != oldBrand?.Slug)
            await _cacheService.RemoveAsync($"brand:slug:{brand.Slug}");
        return brand;
    }

    public async Task DeleteAsync(Guid id)
    {
        var brand = await _brandService.GetByIdAsync(id);
        await _brandService.DeleteAsync(id);
        await _cacheService.RemoveAsync($"brand:slug:{brand.Slug}");
        await _cacheService.RemoveAsync($"brand:id:{brand.Id}");
    }
}