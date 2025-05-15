using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.interfaces;

namespace TallerIdwm.src.data
{
    public class UnitOfWork(StoreContext context, IProductRepository productRepository, IUserRepository userRepository, IBasketRepository basketRepository)
    {

        private readonly StoreContext _context = context;

        public IUserRepository UserRepository { get; set; } = userRepository;

        public IProductRepository ProductRepository { get; set; } = productRepository;
        public IBasketRepository BasketRepository { get; set; } = basketRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}