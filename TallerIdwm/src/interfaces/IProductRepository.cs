using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);

        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> AddProductAsync(Product product);

         void DeleteProductAsync(Product product);

        Task<Product> UpdateProductAsync(Product product);


    }
}