namespace Catalog.API.Infrastructure.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Slug).IsRequired().HasMaxLength(100);
    }
}
