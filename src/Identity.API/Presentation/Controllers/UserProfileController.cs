namespace Identity.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _profileService;

    public UserProfileController(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPaged([FromQuery] UserProfileFilterParams filter)
    {
        var result = await _profileService.GetPagedAsync(filter);
        return Ok(new ApiResponse<PagedResult<UserProfileResponse>>
        {
            Data = result,
            Success = true,
            Message = "User profiles retrieved successfully"
        });
    }

    [HttpGet("my-profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var result = await _profileService.GetByUserIdAsync(userId.Value);
        return Ok(new ApiResponse<UserProfileResponse>
        {
            Data = result,
            Success = true,
            Message = "User profile retrieved successfully"
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _profileService.GetByIdAsync(id);
        return Ok(new ApiResponse<UserProfileResponse>
        {
            Data = result,
            Success = true,
            Message = "User profile retrieved successfully"
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] UserProfileRequest request)
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var isAdmin = User.IsInRole("Admin");
        var result = await _profileService.CreateAsync(userId.Value, request, isAdmin);
        return Ok(new ApiResponse<UserProfileResponse>
        {
            Data = result,
            Success = true,
            Message = "User profile created successfully"
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UserProfileUpdateRequest request)
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var isAdmin = User.IsInRole("Admin");
        var result = await _profileService.UpdateAsync(userId.Value, request, isAdmin);
        return Ok(new ApiResponse<UserProfileResponse>
        {
            Data = result,
            Success = true,
            Message = "User profile updated successfully"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var isAdmin = User.IsInRole("Admin");
        await _profileService.DeleteAsync(id, userId.Value, isAdmin);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User profile deleted successfully"
        });
    }
}
