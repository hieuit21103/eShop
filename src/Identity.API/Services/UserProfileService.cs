using Microsoft.EntityFrameworkCore;
using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Services.Interfaces;
using Identity.API.Models.DTOs;

namespace Identity.API.Services
{
    public class UserProfileService : GenericService<UserProfile>, IGenericService<UserProfile>
    {
        public UserProfileService(ApplicationDbContext context) : base(context){}
    }
}