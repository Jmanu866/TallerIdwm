using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIdwm.src.data;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.models;
using Microsoft.EntityFrameworkCore;

namespace TallerIdwm.src.repositories
{
    public class OrderRepository(StoreContext context) : IOrderRepository
    {
       private readonly StoreContext _context = context;

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }


        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public Task<Order?> GetOrderByIdAsync(int orderId, string userId)
        {
            throw new NotImplementedException();
        }
    }
   
}