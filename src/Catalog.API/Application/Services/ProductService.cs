namespace Catalog.API.Application.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IMapper _mapper;

    public ProductService(IRepository<Product> productRepository, IFileUploadService fileUploadService, IMapper mapper)
    {
        _productRepository = productRepository;
        _fileUploadService = fileUploadService;
        _mapper = mapper;
    }

    public async Task<ProductResponse> GetByIdAsync(Guid id)
    {
        var spec = new ProductByIdSpecification(id);
        var product = await _productRepository.FirstOrDefaultAsync(spec);
        var response = _mapper.Map<ProductResponse>(product);

        foreach (var image in response.ProductImages)
        {
            image.ImageUrl = await _fileUploadService.GetPresignedUrlAsync(image.ImageUrl);
        }

        return response;
    }

    public async Task<ProductResponse> GetBySlugAsync(string slug)
    {
        var spec = new ProductBySlugSpecification(slug);
        var product = await _productRepository.FirstOrDefaultAsync(spec);
        var response = _mapper.Map<ProductResponse>(product);

        foreach (var image in response.ProductImages)
        {
            image.ImageUrl = await _fileUploadService.GetPresignedUrlAsync(image.ImageUrl);
        }

        return response;
    }

    public async Task<PagedResult<ProductResponse>> GetAllAsync(ProductFilterParams filterParams)
    {
        var spec = new ProductFilterSpecification(filterParams);
        var count = await _productRepository.CountAsync();
        var products = await _productRepository.ListAsync(spec);
        var responses = _mapper.Map<IEnumerable<ProductResponse>>(products).ToList();

        return new PagedResult<ProductResponse>
        {
            Items = responses,
            TotalCount = count,
            CurrentPage = filterParams.PageNumber,
            PageSize = filterParams.PageSize,
            TotalPages = (int)Math.Ceiling((double)count / filterParams.PageSize)
        };
    }

    public async Task<ProductResponse> CreateAsync(ProductRequest request)
    {
        var product = _mapper.Map<Product>(request);

        if (request.Images != null && request.Images.Any())
        {
            var imageUrls = await _fileUploadService.UploadImagesAsync(request.Images);
            foreach (var url in imageUrls)
            {
                product.ProductImages.Add(new ProductImages
                {
                    ImageId = url,
                    IsPrimary = product.ProductImages.Count == 0
                });
            }
        }

        await _productRepository.AddAsync(product);

        var spec = new ProductByIdSpecification(product.Id);
        var createdProduct = await _productRepository.FirstOrDefaultAsync(spec);
        var response = _mapper.Map<ProductResponse>(createdProduct);

        foreach (var image in response.ProductImages)
        {
            image.ImageUrl = await _fileUploadService.GetPresignedUrlAsync(image.ImageUrl);
        }

        return response;
    }

    public async Task<ProductResponse> UpdateAsync(ProductUpdateRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
        }

        _mapper.Map(request, product);
        if (request.Images != null && request.Images.Any())
        {
            var imageUrls = await _fileUploadService.UploadImagesAsync(request.Images);
            product.ProductImages.Clear();
            foreach (var url in imageUrls)
            {
                product.ProductImages.Add(new ProductImages
                {
                    ImageId = url,
                    IsPrimary = product.ProductImages.Count == 0
                });
            }
        }

        await _productRepository.UpdateAsync(product);

        var spec = new ProductByIdSpecification(product.Id);
        var updatedProduct = await _productRepository.FirstOrDefaultAsync(spec);
        var response = _mapper.Map<ProductResponse>(updatedProduct);

        foreach (var image in response.ProductImages)
        {
            image.ImageUrl = await _fileUploadService.GetPresignedUrlAsync(image.ImageUrl);
        }

        return response;
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            await _productRepository.DeleteAsync(product);
        }
    }
}
