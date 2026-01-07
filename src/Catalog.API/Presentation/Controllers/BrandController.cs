namespace Catalog.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] BrandFilterParams filterParams)
    {
        var brands = await _brandService.GetAllAsync(filterParams);
        return Ok(new ApiResponse<PagedResult<BrandResponse>>
        {
            Data = brands,
            Success = true,
            Message = "Brands retrieved successfully"
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var brand = await _brandService.GetByIdAsync(id);
        if (brand == null)
            return NotFound();

        return Ok(new ApiResponse<BrandResponse>
        {
            Data = brand,
            Success = true,
            Message = "Brand retrieved successfully"
        });
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult> GetBySlug(string slug)
    {
        var brand = await _brandService.GetBySlugAsync(slug);
        if (brand == null)
            return NotFound();

        return Ok(new ApiResponse<BrandResponse>
        {
            Data = brand,
            Success = true,
            Message = "Brand retrieved successfully"
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Create([FromForm] BrandRequest request)
    {
        var brand = await _brandService.CreateAsync(request);
        return Ok(new ApiResponse<BrandResponse>
        {
            Data = brand,
            Success = true,
            Message = "Brand created successfully"
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Update(Guid id, [FromForm] BrandUpdateRequest request)
    {
        if (id != request.Id)
            return BadRequest("ID mismatch");

        try
        {
            var brand = await _brandService.UpdateAsync(request);
            return Ok(new ApiResponse<BrandResponse>
            {
                Data = brand,
                Success = true,
                Message = "Brand updated successfully"
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
        await _brandService.DeleteAsync(id);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Brand deleted successfully"
        });
    }
}
