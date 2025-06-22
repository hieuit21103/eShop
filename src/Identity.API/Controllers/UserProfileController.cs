using Identity.API.Services.Interfaces;
using Identity.API.Models;
using Identity.API.Models.DTOs;

namespace Identity.API.Controllers
{
    public class UserProfileController : BaseController<UserProfile>
    {
        public UserProfileController(IGenericService<UserProfile> userProfileService) : base(userProfileService){}
    }
}
    