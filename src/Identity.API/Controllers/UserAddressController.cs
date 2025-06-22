using Identity.API.Services.Interfaces;
using Identity.API.Models;
using Identity.API.Models.DTOs;

namespace Identity.API.Controllers
{
    public class UserAddressController : BaseController<UserAddress>
    {
        public UserAddressController(IGenericService<UserAddress> userAddressService) : base(userAddressService){}
    }
}
    