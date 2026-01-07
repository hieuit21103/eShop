namespace Catalog.API.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired();

        builder.Property(p => p.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
