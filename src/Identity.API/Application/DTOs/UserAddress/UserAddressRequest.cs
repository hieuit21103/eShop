namespace Identity.API.Application.DTOs.UserAddress;

public class UserAddressRequest
{
    public Guid? UserId { get; set; }
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string AddressLine { get; set; }
    public required string Ward { get; set; }
    public required string District { get; set; }
    public required string City { get; set; }
    public bool IsDefault { get; set; } = false;
}