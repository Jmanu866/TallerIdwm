using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.repositories
{
    public class UserRepository(StoreDbContext context) : IUserRepository
    {
        private readonly StoreDbContext _context = store;

    
        public async Task<User> CreateUserAsync(User user, ShippingAddress? shippingAddress)
        {
            await _context.Users.AddAsync(user);
            if (shippingAddress != null)
            {
                await _context.ShippingAddresses.AddAsync(shippingAddress);
            }
        }

        public void DeleteUserAsync(User user, ShippingAddress shippingAddress)
        {
            _context.Users.Remove(user);
            _context.ShippingAddresses.Remove(shippingAddress);
        }


       
    }
    {
        
    }
}