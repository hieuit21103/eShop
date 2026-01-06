using Identity.API.Application.DTOs.UserProfile;
using Identity.API.Domain.Filters;

namespace Identity.API.Application.Interfaces;

public interface IUserProfileService
{
    Task<UserProfileResponse?> GetByUserIdAsync(Guid userId);
    Task<PagedResult<UserProfileResponse>> GetPagedAsync(UserProfileFilterParams filter);
    Task<UserProfileResponse> GetByIdAsync(Guid id);
    Task<UserProfileResponse> CreateAsync(Guid userId, UserProfileRequest request, bool isAdmin = false);
    Task<UserProfileResponse> UpdateAsync(Guid userId, UserProfileUpdateRequest request, bool isAdmin = false);
    Task DeleteAsync(Guid id, Guid userId, bool isAdmin = false);
}
