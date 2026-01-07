namespace Catalog.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] ProductFilterParams filterParams)
    {
        var products = await _productService.GetAllAsync(filterParams);
        return Ok(new ApiResponse<PagedResult<ProductResponse>>
        {
            Data = products,
            Success = true,
            Message = "Products retrieved successfully"
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(new ApiResponse<ProductResponse>
        {
            Data = product,
            Success = true,
            Message = "Product retrieved successfully"
        });
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult> GetBySlug(string slug)
    {
        var product = await _productService.GetBySlugAsync(slug);
        if (product == null)
            return NotFound();

        return Ok(new ApiResponse<ProductResponse>
        {
            Data = product,
            Success = true,
            Message = "Product retrieved successfully"
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Create([FromForm] ProductRequest request)
    {
        var product = await _productService.CreateAsync(request);
        return Ok(new ApiResponse<ProductResponse>
        {
            Data = product,
            Success = true,
            Message = "Product created successfully"
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Update(Guid id, [FromForm] ProductUpdateRequest request)
    {
        if (id != request.Id)
            return BadRequest("ID mismatch");

        try
        {
            var product = await _productService.UpdateAsync(request);
            return Ok(new ApiResponse<ProductResponse>
            {
                Data = product,
                Success = true,
                Message = "Product updated successfully"
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
        await _productService.DeleteAsync(id);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Product deleted successfully"
        });
    }
}
