using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserAddressController : ControllerBase
{
    private readonly IUserAddressService _addressService;

    public UserAddressController(IUserAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] UserAddressFilterParams filter)
    {
        var result = await _addressService.GetPagedAsync(filter);
        return Ok(new ApiResponse<PagedResult<UserAddressResponse>>
        {
            Data = result,
            Success = true,
            Message = "Addresses retrieved successfully"
        });
    }

    [HttpGet("my-addresses")]
    public async Task<IActionResult> GetMyAddresses()
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) 
            return Unauthorized(new ApiResponse
            {
                Success = false,
                Message = "User is not authorized"
            });
        var result = await _addressService.GetByUserIdAsync(userId.Value);
        return Ok(new ApiResponse<IEnumerable<UserAddressResponse>>
        {
            Data = result,
            Success = true,
            Message = "User addresses retrieved successfully"
        });
    }

    [HttpGet("my-default")]
    public async Task<IActionResult> GetMyDefaultAddress()
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized(new ApiResponse
        {
            Success = false,
            Message = "User is not authorized"
        });
        var result = await _addressService.GetDefaultByUserIdAsync(userId.Value);
        return Ok(new ApiResponse<UserAddressResponse>
        {
            Data = result,
            Success = true,
            Message = "User default address retrieved successfully"
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _addressService.GetByIdAsync(id);
        return Ok(new ApiResponse<UserAddressResponse>
        {
            Data = result,
            Success = true,
            Message = "Address retrieved successfully"
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserAddressRequest request)
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var isAdmin = User.IsInRole("Admin");
        var result = await _addressService.CreateAsync(userId.Value, request, isAdmin);
        return Ok(new ApiResponse<UserAddressResponse>
        {
            Data = result,
            Success = true,
            Message = "Address created successfully"
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserAddressUpdateRequest request)
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var isAdmin = User.IsInRole("Admin");
        var result = await _addressService.UpdateAsync(userId.Value, request, isAdmin);
        return Ok(new ApiResponse<UserAddressResponse>
        {
            Data = result,
            Success = true,
            Message = "Address updated successfully"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var isAdmin = User.IsInRole("Admin");
        await _addressService.DeleteAsync(id, userId.Value, isAdmin);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Address deleted successfully"
        });
    }
}
