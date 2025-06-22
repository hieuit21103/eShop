using Identity.API.Services.Interfaces;
using Identity.API.Models;

namespace Identity.API.Controllers
{
    public class UserAddressController : BaseController<UserAddress>
    {
        public UserAddressController(IGenericService<UserAddress> userAddressService) : base(userAddressService)
        {
            if (userAddressService == null)
            {
                throw new ArgumentNullException(nameof(userAddressService));
            }   
        }
    }
}
    