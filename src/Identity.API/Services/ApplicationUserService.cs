using Identity.API.Models;
using Identity.API.Data;
using Identity.API.Services.Interfaces;

namespace Identity.API.Services
{
    public class ApplicationUserService : GenericService<ApplicationUser>, IGenericService<ApplicationUser>
    {
        public ApplicationUserService(ApplicationDbContext context) : base(context){ }
    }
}
