namespace Identity.API.Application.Interfaces;

public interface IApplicationUserService
{
    Task<PagedResult<ApplicationUserResponse>> GetPagedAsync(ApplicationUserFilterParams filter);
    Task<ApplicationUserResponse> GetByIdAsync(string id);
    Task<ApplicationUserResponse> GetByUsernameAsync(string username);
    Task<ApplicationUserResponse> GetByEmailAsync(string email);
    Task<ApplicationUserResponse> GetCurrentUserAsync(Guid userId);
    Task<ApplicationUserResponse> CreateAsync(ApplicationUserRequest user);
    Task<ApplicationUserResponse> UpdateAsync(ApplicationUserUpdateRequest user);
    Task DeleteAsync(string id);
    Task AssignRolesAsync(string userId, IEnumerable<string> roles);
}
