namespace Identity.API.Services
{
    public class UserProfileService : GenericService<UserProfile>, IGenericService<UserProfile>
    {
        public UserProfileService(ApplicationDbContext context) : base(context){}
    }
}