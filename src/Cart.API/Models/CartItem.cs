using System.ComponentModel.DataAnnotations;

namespace Cart.API.Models;

public class CartItem : IValidatableObject
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string PictureUrl { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Quantity <= 0)
        {
            yield return new ValidationResult("Quantity must be greater than zero.", new[] { nameof(Quantity) });
        }

        if (UnitPrice < 0)
        {
            yield return new ValidationResult("Unit price cannot be negative.", new[] { nameof(UnitPrice) });
        }
    }
}