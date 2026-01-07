namespace Catalog.API.Domain.Filters;

public class ProductFilterParams : FilterParams
{
    public Guid? BrandId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
