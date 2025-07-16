namespace Identity.API.Services
{
    public class ApplicationUserService : GenericService<ApplicationUser>, IGenericService<ApplicationUser>
    {
        public ApplicationUserService(ApplicationDbContext context) : base(context){ }
    }
}
