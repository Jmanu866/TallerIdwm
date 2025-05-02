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
                PostalCode = user.ShippingAddress?.PostalCode ?? string.Empty
            };
    
    }
}