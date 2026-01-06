namespace Identity.API.Domain.Filters;

public class UserAddressFilterParams : FilterParams
{
    public Guid? UserId { get; set; }
    public string? FullName { get; set; }
    public string? City { get; set; }
}