namespace Identity.API.Services
{
    public class UserAddressService : GenericService<UserAddress>, IGenericService<UserAddress>
    {
        private readonly ApplicationDbContext _context;
        public UserAddressService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}