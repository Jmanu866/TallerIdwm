using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.models;
using TallerIdwm.src.dtos;

namespace TallerIdwm.src.mappers
{
    public class UserMapper
    {
        public static User RegisterToUser(RegisterDto dto) =>
            new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.Phone,
                Phone = dto.Phone,
                RegisteredAt = DateTime.UtcNow,
                IsActive = true,
                ShippingAddress = new ShippingAddress
                {
                    Street = dto.Street ?? string.Empty,
                    Number = dto.Number ?? string.Empty,
                    Commune = dto.Commune ?? string.Empty,
                    Region = dto.Region ?? string.Empty,
                    PostalCode = dto.PostalCode ?? string.Empty
                }
            };
        public static UserDto UserToUserDto(User user) =>
            new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                Street = user.ShippingAddress?.Street,
                Number = user.ShippingAddress?.Number,
                Commune = user.ShippingAddress?.Commune,
                Region = user.ShippingAddress?.Region,
                PostalCode = user.ShippingAddress?.PostalCode,
                RegisteredAt = user.RegisteredAt,
                LastAccess = user.LastAccess,
                IsActive = user.IsActive
            };
        public static AuthenticateUserDto UserToAuthenticatedDto(User user, string token) =>
            new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                Token = token,
                Street = user.ShippingAddress?.Street,
                Number = user.ShippingAddress?.Number,
                Commune = user.ShippingAddress?.Commune,
                Region = user.ShippingAddress?.Region,
                PostalCode = user.ShippingAddress?.PostalCode,
                RegisteredAt = user.RegisteredAt,
                LastAccess = user.LastAccess,
                IsActive = user.IsActive
            };
    }
}
