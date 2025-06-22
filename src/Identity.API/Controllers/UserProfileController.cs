using Identity.API.Services.Interfaces;
using Identity.API.Models;
using Identity.API.Models.DTOs;

namespace Identity.API.Controllers
{
    public class UserProfileController : BaseController<UserProfileDto>
    {
        public UserProfileController(IGenericService<UserProfileDto> userProfileService) : base(userProfileService)
        {
            if (userProfileService == null)
            {
                throw new ArgumentNullException(nameof(userProfileService));
            }   
        }
    }
}
    