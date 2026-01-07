namespace Catalog.API.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : string.Empty))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.ProductImages.Count));

        CreateMap<ProductImages, ProductImageResponse>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageId));

        CreateMap<ProductRequest, Product>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));

        CreateMap<ProductUpdateRequest, Product>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));

        CreateMap<Brand, BrandResponse>();
        CreateMap<BrandRequest, Brand>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));
        CreateMap<BrandUpdateRequest, Brand>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));

        CreateMap<Category, CategoryResponse>();
        CreateMap<CategoryRequest, Category>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));
        CreateMap<CategoryUpdateRequest, Category>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));
    }
}
