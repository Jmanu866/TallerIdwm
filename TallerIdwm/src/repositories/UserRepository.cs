using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TallerIdwm.src.dtos;
using TallerIdwm.src.models;
using TallerIdwm.src.mappers;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.data;

namespace TallerIdwm.src.repositories
{
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        public IQueryable<User> GetUsersQueryable()
        {
            return _userManager.Users.Include(u => u.ShippingAddress).AsQueryable();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.Users
                .Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users
                .Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.Users
                .Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await Task.Run(() =>
            {
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash!, password);
                return result == PasswordVerificationResult.Success;
            });
        }
        public async Task<IdentityResult> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
        => await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);


        public Task<User?> GetUserWithAddressByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

    }
}
