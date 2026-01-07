namespace Catalog.API.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> GetByIdAsync(Guid id)
    {
        var spec = new CategoryByIdSpecification(id);
        var category = await _categoryRepository.FirstOrDefaultAsync(spec);
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse> GetBySlugAsync(string slug)
    {
        var spec = new CategoryBySlugSpecification(slug);
        var category = await _categoryRepository.FirstOrDefaultAsync(spec);
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<PagedResult<CategoryResponse>> GetAllAsync(CategoryFilterParams filterParams)
    {
        var spec = new CategoryFilterSpecification(filterParams);
        var count = await _categoryRepository.CountAsync();
        var categories = await _categoryRepository.ListAsync(spec);
        return new PagedResult<CategoryResponse>
        {
            Items = _mapper.Map<IEnumerable<CategoryResponse>>(categories),
            TotalCount = count,
            CurrentPage = filterParams.PageNumber,
            PageSize = filterParams.PageSize,
            TotalPages = (int)Math.Ceiling((double)count / filterParams.PageSize)
        };
    }

    public async Task<CategoryResponse> CreateAsync(CategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);
        await _categoryRepository.AddAsync(category);
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
        }

        _mapper.Map(request, category);
        await _categoryRepository.UpdateAsync(category);
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category != null)
        {
            await _categoryRepository.DeleteAsync(category);
        }
    }
}
