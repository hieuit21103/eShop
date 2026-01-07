namespace Catalog.API.Infrastructure.Data.Configurations;

public class ProductImagesConfiguration : IEntityTypeConfiguration<ProductImages>
{
    public void Configure(EntityTypeBuilder<ProductImages> builder)
    {
        builder.ToTable("ProductImages");
        builder.HasKey(pi => pi.Id);
        // Add specific config if needed
    }
}
