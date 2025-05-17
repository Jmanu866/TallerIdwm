using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.models;
using TallerIdwm.src.data;
using TallerIdwm.src.interfaces;

using Microsoft.EntityFrameworkCore;

namespace TallerIdwm.src.repositories
{
    public class ProductRepository(StoreContext store, ILogger<Product> logger) : IProductRepository
    {
        private readonly StoreContext _context = store;
        private readonly ILogger<Product> _logger = logger;

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            _logger.LogWarning("Entrando a GetProductById con id: {Id}", id);
            return await _context.Products.FindAsync(id) ?? throw new Exception("Product not found");
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync() ?? throw new Exception("No products found");
        }
        public IQueryable<Product> GetQueryableProducts()
        {
            return _context.Products.AsQueryable();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id) ?? throw new Exception("Product not found");
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Urls = product.Urls;
            existingProduct.Brand = product.Brand;
            _context.Products.Update(existingProduct);
        }
    }
}