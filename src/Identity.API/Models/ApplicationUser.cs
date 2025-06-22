using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        public UserProfile Profile { get; set; } = new UserProfile();
    }
}