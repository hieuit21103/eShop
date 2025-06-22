using Microsoft.EntityFrameworkCore;
using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Services.Interfaces;

namespace Identity.API.Services
{
    public class UserProfileService : GenericService<UserProfile>, IGenericService<UserProfile>
    {
        public UserProfileService(ApplicationDbContext context) : base(context){}
    }
}