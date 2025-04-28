using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.interfaces
{
    public interface IUserRepository
    {
        
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByIdAsync(string FirstName);
        Task CreateUserAsync(User user, ShippingAddress? shippingAddress);

        void UpdateProductAsync(User user);

        void UpdateShippingAddressAsync(UserDtos userDto);

        void DeleteUserAsync(User user, ShippingAddress shippingAddress);


    }
}