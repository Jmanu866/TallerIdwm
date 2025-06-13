using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIdwm.src.models;

namespace TallerIdwm.src.interfaces
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketByIdAsync(string id);
        Basket CreateBasket(string basketId);
        void UpdateBasket(Basket basket);
        void DeleteBasket(Basket basket);
    }
}