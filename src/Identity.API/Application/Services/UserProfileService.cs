using Grpc.Core;
using FileStorage.Protos;

namespace Identity.API.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IRepository<UserProfile> _repository;
    private readonly FileStorageService.FileStorageServiceClient _grpcClient;
    private readonly IMapper _mapper;

    public UserProfileService(
        IRepository<UserProfile> repository,
        FileStorageService.FileStorageServiceClient grpcClient,
        IMapper mapper)
    {
        _repository = repository;
        _grpcClient = grpcClient;
        _mapper = mapper;
    }

    public async Task<UserProfileResponse?> GetByUserIdAsync(Guid userId)
    {
        var spec = new UserProfileByUserIdSpecification(userId);
        var item = await _repository.FirstOrDefaultAsync(spec);
        return item == null ? null : _mapper.Map<UserProfileResponse>(item);
    }

    public async Task<PagedResult<UserProfileResponse>> GetPagedAsync(UserProfileFilterParams filter)
    {
        var filterSpec = new UserProfileFilterSpecification(filter, isPaging: false);
        var totalItems = await _repository.CountAsync(filterSpec);

        var pagedSpec = new UserProfileFilterSpecification(filter, isPaging: true);
        var items = await _repository.ListAsync(pagedSpec);

        return new PagedResult<UserProfileResponse>
        {
            Items = items.Select(item => _mapper.Map<UserProfileResponse>(item)).ToList(),
            TotalCount = totalItems,
            CurrentPage = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize)
        };
    }

    public async Task<UserProfileResponse> GetByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("User profile not found.");
        return _mapper.Map<UserProfileResponse>(item);
    }

    public async Task<UserProfileResponse> CreateAsync(Guid userId, UserProfileRequest request, bool isAdmin = false)
    {
        if (!isAdmin && userId != request.UserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to create profile for this user.");
        }

        var existingSpec = new UserProfileByUserIdSpecification(userId);
        var existingProfile = await _repository.FirstOrDefaultAsync(existingSpec);
        if (existingProfile != null)
        {
            throw new InvalidOperationException("User profile already exists.");
        }

        var userProfile = new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            CreatedAt = DateTime.UtcNow
        };

        if (request.Avatar != null)
        {
            var response = await UploadAvatarAsync(request.Avatar);
            userProfile.AvatarId = Guid.Parse(response.Id);
        }

        await _repository.AddAsync(userProfile);
        await _repository.SaveChangesAsync();
        return _mapper.Map<UserProfileResponse>(userProfile);
    }

    public async Task<UserProfileResponse> UpdateAsync(Guid userId, UserProfileUpdateRequest request, bool isAdmin = false)
    {
        var spec = new UserProfileByUserIdSpecification(userId);
        var userProfile = await _repository.FirstOrDefaultAsync(spec)
                          ?? throw new KeyNotFoundException("User profile not found.");

        if (userProfile.UserId != userId && !isAdmin)
            throw new UnauthorizedAccessException("You do not have permission to update this profile.");

        if (request.FullName != null) userProfile.FullName = request.FullName;
        if (request.DateOfBirth != null) userProfile.DateOfBirth = request.DateOfBirth;
        if (request.Gender != null) userProfile.Gender = request.Gender;
        if (request.Avatar != null)
        {
            if (userProfile.AvatarId.HasValue)
            {
                await _grpcClient.DeleteFileAsync(new FileRequest { Id = userProfile.AvatarId.ToString() });
            }
            var response = await UploadAvatarAsync(request.Avatar);
            userProfile.AvatarId = Guid.Parse(response.Id);
        }

        userProfile.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(userProfile);
        await _repository.SaveChangesAsync();

        return _mapper.Map<UserProfileResponse>(userProfile);
    }

    public async Task DeleteAsync(Guid id, Guid userId, bool isAdmin = false)
    {
        var userProfile = await _repository.GetByIdAsync(id)
                          ?? throw new KeyNotFoundException("User profile not found.");

        if (userProfile.UserId != userId && !isAdmin)
            throw new UnauthorizedAccessException("You do not have permission to delete this profile.");
        if (userProfile.AvatarId.HasValue)
        {
            await _grpcClient.DeleteFileAsync(new FileRequest { Id = userProfile.AvatarId.ToString() });
        }
        await _repository.DeleteAsync(userProfile);
        await _repository.SaveChangesAsync();
    }

    private async Task<FileUploadResponse> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        using var call = _grpcClient.UploadFile();

        await call.RequestStream.WriteAsync(new UploadFileRequest
        {
            Metadata = new FileUploadMetadata
            {
                FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
                FilePath = "avatars/"
            }
        });

        await file.StreamContentToGrpcAsync(
            call.RequestStream,
            chunk => new UploadFileRequest { ChunkData = chunk },
            cancellationToken);

        await call.RequestStream.CompleteAsync();

        var response = await call.ResponseAsync;
        return response;
    }
}
