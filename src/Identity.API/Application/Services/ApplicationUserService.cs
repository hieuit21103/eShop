namespace Identity.API.Application.Services;

public class ApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;

    public ApplicationUserService(
        UserManager<ApplicationUser> userManager,
        IRepository<ApplicationUser> userRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ApplicationUserResponse>> GetPagedAsync(ApplicationUserFilterParams filter)
    {
        var spec = new UserWithDetailFilterSpecification(filter, isPaging: true);
        var totalItems = await _userRepository.CountAsync();
        var items = await _userRepository.ListAsync(spec);
        var responses = items.Select(user => _mapper.Map<ApplicationUserResponse>(user)).ToList();

        return new PagedResult<ApplicationUserResponse>
        {
            Items = responses,
            CurrentPage = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize),
            TotalCount = totalItems
        };
    }

    public async Task<ApplicationUserResponse> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID '{id}' was not found.");
        }
        return _mapper.Map<ApplicationUserResponse>(user);
    }

    public async Task<ApplicationUserResponse> GetByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with username '{username}' was not found.");
        }
        return _mapper.Map<ApplicationUserResponse>(user);
    }

    public async Task<ApplicationUserResponse> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with email '{email}' was not found.");
        }
        return _mapper.Map<ApplicationUserResponse>(user);
    }

    public async Task<ApplicationUserResponse> GetCurrentUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID '{userId}' was not found.");
        }
        return _mapper.Map<ApplicationUserResponse>(user);
    }

    public async Task<ApplicationUserResponse> CreateAsync(ApplicationUserRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return _mapper.Map<ApplicationUserResponse>(user);
    }

    public async Task<ApplicationUserResponse> UpdateAsync(ApplicationUserUpdateRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id) ?? throw new KeyNotFoundException($"User with ID '{request.Id}' was not found.");

        user.Email = request.Email;
        user.UserName = request.UserName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return _mapper.Map<ApplicationUserResponse>(user);
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new KeyNotFoundException($"User with ID '{id}' was not found.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    public async Task AssignRolesAsync(string userId, IEnumerable<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        var rolesToAdd = roles.Except(currentRoles);
        var rolesToRemove = currentRoles.Except(roles);

        var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
        if (!addResult.Succeeded)
        {
            throw new InvalidOperationException($"Failed to add roles: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
        }

        var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        if (!removeResult.Succeeded)
        {
            throw new InvalidOperationException($"Failed to remove roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
        }
    }
}
