using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.models;
using TallerIdwm.src.dtos;


namespace TallerIdwm.src.interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsersQueryable();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string firstName);
        Task CreateUserAsync(User user, ShippingAddress? shippingAddress);
        Task UpdateUserAsync(User user);
        Task UpdateShippingAddressAsync(UserDto userDto);
        // Task DeleteUserAsync(User user, ShippingAddress shippingAddress);

    }
}