using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIdwm.src.models;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.data;
using Microsoft.EntityFrameworkCore;

namespace TallerIdwm.src.repositories
{
    public class BasketRepository(StoreContext context) : IBasketRepository
    {
        private readonly StoreContext _context = context;

        public async Task<Basket?> GetBasketByIdAsync(string basketId)
        {
            var basket = await _context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.BasketId == basketId);

            return basket;
        }

        public Basket CreateBasket(string basketId)
        {
            var basket = new Basket { BasketId = basketId };
            _context.Baskets.Add(basket);
            return basket;
        }
        public void UpdateBasket(Basket basket)
        {
            _context.Baskets.Update(basket);
        }
        public void DeleteBasket(Basket basket)
        {
            _context.Baskets.Remove(basket);
        }


    }

}