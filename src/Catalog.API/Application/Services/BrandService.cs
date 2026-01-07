namespace Catalog.API.Application.Services;

public class BrandService : IBrandService
{
    private readonly IRepository<Brand> _brandRepository;
    private readonly ICacheService _cacheService;
    private readonly IFileUploadService _fileUploadService;
    private readonly IMapper _mapper;

    public BrandService(
        IRepository<Brand> brandRepository,
        ICacheService cacheService,
        IFileUploadService fileUploadService,
        IMapper mapper)
    {
        _brandRepository = brandRepository;
        _cacheService = cacheService;
        _fileUploadService = fileUploadService;
        _mapper = mapper;
    }

    public async Task<BrandResponse> GetByIdAsync(Guid id)
    {
        var spec = new BrandByIdSpecification(id);
        var brand = await _brandRepository.FirstOrDefaultAsync(spec);
        var response = _mapper.Map<BrandResponse>(brand);

        if (!string.IsNullOrEmpty(response.Image))
        {
            response.Image = await _fileUploadService.GetPresignedUrlAsync(response.Image);
        }

        return response;
    }

    public async Task<BrandResponse> GetBySlugAsync(string slug)
    {
        var spec = new BrandBySlugSpecification(slug);
        var brand = await _brandRepository.FirstOrDefaultAsync(spec);
        var response = _mapper.Map<BrandResponse>(brand);

        if (!string.IsNullOrEmpty(response.Image))
        {
            response.Image = await _fileUploadService.GetPresignedUrlAsync(response.Image);
        }

        return response;
    }

    public async Task<PagedResult<BrandResponse>> GetAllAsync(BrandFilterParams filterParams)
    {
        var spec = new BrandFilterSpecification(filterParams);
        var count = await _brandRepository.CountAsync();
        var brands = await _brandRepository.ListAsync(spec);
        var responses = _mapper.Map<IEnumerable<BrandResponse>>(brands).ToList();

        return new PagedResult<BrandResponse>
        {
            Items = responses,
            TotalCount = count,
            CurrentPage = filterParams.PageNumber,
            PageSize = filterParams.PageSize,
            TotalPages = (int)Math.Ceiling((double)count / filterParams.PageSize)
        };
    }

    public async Task<BrandResponse> CreateAsync(BrandRequest request)
    {
        var brand = _mapper.Map<Brand>(request);

        if (request.Image != null)
        {
            var imageUrls = await _fileUploadService.UploadImagesAsync(new List<IFormFile> { request.Image });
            if (imageUrls.Any())
            {
                brand.Image = imageUrls.First();
            }
        }

        await _brandRepository.AddAsync(brand);
        var response = _mapper.Map<BrandResponse>(brand);

        if (!string.IsNullOrEmpty(response.Image))
        {
            response.Image = await _fileUploadService.GetPresignedUrlAsync(response.Image);
        }

        return response;
    }

    public async Task<BrandResponse> UpdateAsync(BrandUpdateRequest request)
    {
        var brand = await _brandRepository.GetByIdAsync(request.Id);
        if (brand == null)
        {
            throw new KeyNotFoundException($"Brand with ID {request.Id} not found.");
        }

        _mapper.Map(request, brand);

        if (request.Picture != null)
        {
            var imageUrls = await _fileUploadService.UploadImagesAsync(new List<Microsoft.AspNetCore.Http.IFormFile> { request.Picture });
            if (imageUrls.Any())
            {
                brand.Image = imageUrls.First();
            }
        }

        await _brandRepository.UpdateAsync(brand);
        var response = _mapper.Map<BrandResponse>(brand);

        if (!string.IsNullOrEmpty(response.Image))
        {
            response.Image = await _fileUploadService.GetPresignedUrlAsync(response.Image);
        }

        return response;
    }

    public async Task DeleteAsync(Guid id)
    {
        var brand = await _brandRepository.GetByIdAsync(id);
        if (brand != null)
        {
            await _brandRepository.DeleteAsync(brand);
        }
    }
}
