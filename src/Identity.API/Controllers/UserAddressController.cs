using Identity.API.Services.Interfaces;
using Identity.API.Models;
using Identity.API.Models.DTOs;

namespace Identity.API.Controllers
{
    public class UserAddressController : BaseController<UserAddressDto>
    {
        public UserAddressController(IGenericService<UserAddressDto> userAddressService) : base(userAddressService)
        {
            if (userAddressService == null)
            {
                throw new ArgumentNullException(nameof(userAddressService));
            }   
        }
    }
}
    