namespace Catalog.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] CategoryFilterParams filterParams)
    {
        var categories = await _categoryService.GetAllAsync(filterParams);
        return Ok(new ApiResponse<PagedResult<CategoryResponse>>
        {
            Data = categories,
            Success = true,
            Message = "Categories retrieved successfully"
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        return Ok(new ApiResponse<CategoryResponse>
        {
            Data = category,
            Success = true,
            Message = "Category retrieved successfully"
        });
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult> GetBySlug(string slug)
    {
        var category = await _categoryService.GetBySlugAsync(slug);
        if (category == null)
            return NotFound();

        return Ok(new ApiResponse<CategoryResponse>
        {
            Data = category,
            Success = true,
            Message = "Category retrieved successfully"
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Create([FromBody] CategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request);
        return Ok(new ApiResponse<CategoryResponse>
        {
            Data = category,
            Success = true,
            Message = "Category created successfully"
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Update(Guid id, [FromBody] CategoryUpdateRequest request)
    {
        if (id != request.Id)
            return BadRequest("ID mismatch");

        try
        {
            var category = await _categoryService.UpdateAsync(request);
            return Ok(new ApiResponse<CategoryResponse>
            {
                Data = category,
                Success = true,
                Message = "Category updated successfully"
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Category deleted successfully"
        });
    }
}
