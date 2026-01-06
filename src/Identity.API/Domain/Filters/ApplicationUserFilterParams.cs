namespace Identity.API.Domain.Filters;

public class ApplicationUserFilterParams : FilterParams
{
    public string? RoleName { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool? PhoneNumberConfirmed { get; set; }
}
