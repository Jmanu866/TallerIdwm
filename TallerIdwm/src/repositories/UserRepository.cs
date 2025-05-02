using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TallerIdwm.src.dtos;
using TallerIdwm.src.models;
using TallerIdwm.src.mappers;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.data;

namespace TallerIdwm.src.repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreContext _context;

        public UserRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.Include(u => u.ShippingAddress).ToListAsync();
            return users.Select(UserMapper.MapToDto);
        }

        public async Task<UserDto> GetUserByIdAsync(string firstName)
        {
            var user = await _context.Users.Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync(u => u.FirstName == firstName)
                ?? throw new Exception("User not found");

            return UserMapper.MapToDto(user);
        }

        public async Task CreateUserAsync(User user, ShippingAddress? shippingAddress)
        {
            await _context.Users.AddAsync(user);
            if (shippingAddress != null)
            {
                await _context.ShippingAddresses.AddAsync(shippingAddress);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShippingAddressAsync(UserDto userDto)
        {
            var user = await _context.Users.Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync(u => u.FirstName == userDto.FirstName)
                ?? throw new Exception("User not found");

            if (user.ShippingAddress == null)
            {
                user.ShippingAddress = new ShippingAddress
                {
                    Street = userDto.Street ?? string.Empty,
                    Number = userDto.Number ?? string.Empty,
                    Commune = userDto.Commune ?? string.Empty,
                    Region = userDto.Region ?? string.Empty,
                    PostalCode = userDto.PostalCode ?? string.Empty
                };
            }
            else
            {
                user.ShippingAddress.Street = userDto.Street ?? string.Empty;
                user.ShippingAddress.Number = userDto.Number ?? string.Empty;
                user.ShippingAddress.Commune = userDto.Commune ?? string.Empty;
                user.ShippingAddress.Region = userDto.Region ?? string.Empty;
                user.ShippingAddress.PostalCode = userDto.PostalCode ?? string.Empty;
            }

            _context.ShippingAddresses.Update(user.ShippingAddress);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user, ShippingAddress shippingAddress)
        {
            _context.Users.Remove(user);
            _context.ShippingAddresses.Remove(shippingAddress);
            await _context.SaveChangesAsync();
        }
    }
}
