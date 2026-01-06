namespace Identity.API.Application.DTOs.UserAddress;

public class UserAddressUpdateRequest
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? AddressLine { get; set; }
    public string? Ward { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public bool? IsDefault { get; set; }
}