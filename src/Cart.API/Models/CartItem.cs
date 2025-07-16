namespace Cart.API.Models;

public class CartItem : IValidatableObject
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string PictureUrl { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ProductId))
        {
            yield return new ValidationResult("ProductId is required.", new[] { nameof(ProductId) });
        }

        if (Quantity <= 0)
        {
            yield return new ValidationResult("Quantity must be greater than zero.", new[] { nameof(Quantity) });
        }

        if (UnitPrice < 0)
        {
            yield return new ValidationResult("UnitPrice cannot be negative.", new[] { nameof(UnitPrice) });
        }
    }
}