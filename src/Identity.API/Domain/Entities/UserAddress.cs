namespace Identity.API.Domain.Entities;

[Table("UserAddresses")]
public class UserAddress
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [Required, MaxLength(255)]
    public string FullName { get; set; } = string.Empty;
    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    [Required, MaxLength(255)]
    public string AddressLine { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string Ward { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string District { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;
    public bool IsDefault { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
