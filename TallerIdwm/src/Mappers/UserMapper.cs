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
        public static UserDto MapToDto(User user) => 
        new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Street = user.ShippingAddress?.Street ?? string.Empty,
                Number = user.ShippingAddress?.Number ?? string.Empty,
                Commune = user.ShippingAddress?.Commune ?? string.Empty,
                Region = user.ShippingAddress?.Region ?? string.Empty,
                PostalCode = user.ShippingAddress?.PostalCode ?? string.Empty,
                RegisteredAt = user.RegisteredAt,
                LastAccess = user.LastAccess,
                IsActive = user.IsActive


            };

        public static User RegisterToUser(RegisterDto dto) =>
            new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirtsName,
                LastName = dto.LastName,
                Password = dto.Password,
                PhoneNumber = dto.Thelephone,
                Phone = dto.Thelephone,
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