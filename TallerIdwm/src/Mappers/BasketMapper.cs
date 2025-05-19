using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIdwm.src.models;
using TallerIdwm.src.dtos;




namespace TallerIdwm.src.mappers
{
    public static class BasketMapper
    {
        public static BasketDto ToDto(this Basket basket)
        {
            return new BasketDto
            {
                BasketId = basket.BasketId,
                Items = [.. basket.Items.Select(x => new BasketItemDto
                {
                    ProductId = x.ProductId,
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    Description = x.Product.Description,
                    Stock = x.Product.Stock,
                    Urls = x.Product.Urls?.Take(1).ToArray() ?? Array.Empty<string>(),
                    Brand = x.Product.Brand,
                    Category = x.Product.Category,
                    Quantity = x.Quantity
                })]
            };
        }
    }
}