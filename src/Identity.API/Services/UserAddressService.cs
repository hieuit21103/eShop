using Microsoft.EntityFrameworkCore;
using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Services.Interfaces;

namespace Identity.API.Services
{
    public class UserAddressService : GenericService<UserAddress>, IGenericService<UserAddress>
    {
        private readonly ApplicationDbContext _context;
        public UserAddressService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAddress>> GetUserAddressesAsync(string userId)
        {
            return await _context.UserAddresses
                .Where(ua => ua.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserAddress> GetUserAddressByIdAsync(Guid addressId)
        {
            return await _context.UserAddresses
                .FirstOrDefaultAsync(ua => ua.Id == addressId) ?? throw new KeyNotFoundException($"Address with id {addressId} not found.");
        }
    }
}