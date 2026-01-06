namespace Identity.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationUserController : ControllerBase
{
    private readonly IApplicationUserService _userService;

    public ApplicationUserController(IApplicationUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPaged([FromQuery] ApplicationUserFilterParams filter)
    {
        var result = await _userService.GetPagedAsync(filter);
        return Ok(new ApiResponse<PagedResult<ApplicationUserResponse>>
        {
            Data = result,
            Success = true,
            Message = "Users retrieved successfully"
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _userService.GetByIdAsync(id);
        return Ok(new ApiResponse<ApplicationUserResponse>
        {
            Data = result,
            Success = true,
            Message = "User retrieved successfully"
        });
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var result = await _userService.GetByUsernameAsync(username);
        return Ok(new ApiResponse<ApplicationUserResponse>
        {
            Data = result,
            Success = true,
            Message = "User retrieved successfully"
        });
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _userService.GetByEmailAsync(email);
        return Ok(new ApiResponse<ApplicationUserResponse>
        {
            Data = result,
            Success = true,
            Message = "User retrieved successfully"
        });
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent()
    {
        var userId = User.GetCurrentUserId();
        if (userId == null)
            return Unauthorized(new ApiResponse 
            { 
                Success = false, 
                Message = "User is not authenticated" 
            });
        var result = await _userService.GetCurrentUserAsync(userId.Value);
        return Ok(new ApiResponse<ApplicationUserResponse>
        {
            Data = result,
            Success = true,
            Message = "Current user retrieved successfully"
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] ApplicationUserRequest request)
    {
        var result = await _userService.CreateAsync(request);
        return Ok(new ApiResponse<ApplicationUserResponse>
        {
            Data = result,
            Success = true,
            Message = "User created successfully"
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ApplicationUserUpdateRequest request)
    {
        var result = await _userService.UpdateAsync(request);
        return Ok(new ApiResponse<ApplicationUserResponse>
        {
            Data = result,
            Success = true,
            Message = "User updated successfully"
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        await _userService.DeleteAsync(id);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User deleted successfully"
        });
    }

    [HttpPost("{userId}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoles(string userId, [FromBody] IEnumerable<string> roles)
    {
        await _userService.AssignRolesAsync(userId, roles);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Roles assigned successfully"
        });
    }
}
