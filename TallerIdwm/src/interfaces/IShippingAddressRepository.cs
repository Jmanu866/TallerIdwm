using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.models;

namespace TallerIdwm.src.interfaces
{
     public interface IShippingAddressRepository
    {
        Task<ShippingAddress?> GetByUserIdAsync(string userId);
        Task AddAsync(ShippingAddress address);
    }
}