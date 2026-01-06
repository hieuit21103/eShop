namespace Identity.API.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserResponse>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(r => r.Role.Name).ToList()));

        CreateMap<UserAddress, UserAddressResponse>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        CreateMap<UserProfile, UserProfileResponse>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
    }
}