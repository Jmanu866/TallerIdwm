using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIdwm.src.models;
using TallerIdwm.src.dtos;

namespace TallerIdwm.src.mappers
{
    public static class ProductMapper
    {
        public static Product FromCreateDto(ProductDto dto, List<string> urls, string? publicId = null)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Category = dto.Category,
                Brand = dto.Brand,
                Stock = dto.Stock,
                Urls = urls,
                PublicId = publicId


            };

        }

    }
}