namespace Identity.API.Application.Interfaces;

public interface IUserAddressService
{
    Task<IEnumerable<UserAddressResponse>> GetByUserIdAsync(Guid userId);
    Task<UserAddressResponse> GetDefaultByUserIdAsync(Guid userId);
    Task<PagedResult<UserAddressResponse>> GetPagedAsync(UserAddressFilterParams filter);
    Task<UserAddressResponse> GetByIdAsync(Guid id);
    Task<UserAddressResponse> CreateAsync(Guid userId, UserAddressRequest request, bool isAdmin = false);
    Task<UserAddressResponse> UpdateAsync(Guid userId, UserAddressUpdateRequest request, bool isAdmin = false);
    Task DeleteAsync(string id, Guid userId, bool isAdmin = false);
}
