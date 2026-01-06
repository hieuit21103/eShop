using Identity.API.Domain.Specifications;

namespace Identity.API.Application.Services;

public class UserAddressService : IUserAddressService
{
    private readonly IRepository<UserAddress> _repository;
    private readonly IMapper _mapper;

    public UserAddressService(IRepository<UserAddress> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserAddressResponse>> GetByUserIdAsync(Guid userId)
    {
        var spec = new UserAddressByUserIdSpecification(userId);
        var items = await _repository.ListAsync(spec);
        return items.Select(item => _mapper.Map<UserAddressResponse>(item));
    }

    public async Task<UserAddressResponse> GetDefaultByUserIdAsync(Guid userId)
    {
        var spec = new UserAddressDefaultSpecification(userId);
        var item = await _repository.FirstOrDefaultAsync(spec) ?? throw new KeyNotFoundException("User address not found.");
        return _mapper.Map<UserAddressResponse>(item);
    }

    public async Task<PagedResult<UserAddressResponse>> GetPagedAsync(UserAddressFilterParams filter)
    {
        var filterSpec = new UserAddressFilterSpecification(filter, isPaging: false);
        var totalItems = await _repository.CountAsync(filterSpec);

        var pagedSpec = new UserAddressFilterSpecification(filter, isPaging: true);
        var items = await _repository.ListAsync(pagedSpec);

        return new PagedResult<UserAddressResponse>
        {
            Items = items.Select(item => _mapper.Map<UserAddressResponse>(item)).ToList(),
            TotalCount = totalItems,
            CurrentPage = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize)
        };
    }

    public async Task<UserAddressResponse> GetByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("User address not found.");
        return _mapper.Map<UserAddressResponse>(item);
    }

    public async Task<UserAddressResponse> CreateAsync(Guid userId, UserAddressRequest request, bool isAdmin = false)
    {
        if(!isAdmin && userId != request.UserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to create address for this user.");
        }
        if (request.IsDefault)
        {
            var spec = new UserAddressDefaultSpecification(request.UserId!.Value);
            var currentDefault = await _repository.FirstOrDefaultAsync(spec);
            if (currentDefault != null)
            {
                currentDefault.IsDefault = false;
                await _repository.UpdateAsync(currentDefault);
            }
        }
        var userAddress = new UserAddress
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FullName = request.FullName,
            Phone = request.Phone,
            AddressLine = request.AddressLine,
            Ward = request.Ward,
            District = request.District,
            City = request.City,
            IsDefault = request.IsDefault,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(userAddress);
        await _repository.SaveChangesAsync();
        return _mapper.Map<UserAddressResponse>(userAddress);
    }

    public async Task<UserAddressResponse> UpdateAsync(Guid userId, UserAddressUpdateRequest request, bool isAdmin = false)
    {
        var userAddress = await _repository.GetByIdAsync(request.Id)
                          ?? throw new KeyNotFoundException("User address not found.");

        if (userAddress.UserId != userId && !isAdmin)
            throw new UnauthorizedAccessException("You do not have permission to update this address.");

        if (request.IsDefault == true)
        {
            var spec = new UserAddressDefaultSpecification(userAddress.UserId);
            var currentDefault = await _repository.FirstOrDefaultAsync(spec);

            if (currentDefault != null && currentDefault.Id != request.Id)
            {
                currentDefault.IsDefault = false;
                await _repository.UpdateAsync(currentDefault);
            }
        }

        _mapper.Map(request, userAddress);

        if (request.IsDefault.HasValue)
        {
            userAddress.IsDefault = request.IsDefault.Value;
        }

        await _repository.UpdateAsync(userAddress);
        await _repository.SaveChangesAsync();
        return _mapper.Map<UserAddressResponse>(userAddress);
    }

    public async Task DeleteAsync(string id, Guid userId, bool isAdmin = false)
    {
        var userAddress = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("User address not found.");
        if (userAddress.UserId != userId && !isAdmin)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this address.");
        }
        await _repository.DeleteAsync(userAddress);
        await _repository.SaveChangesAsync();
    }
}
