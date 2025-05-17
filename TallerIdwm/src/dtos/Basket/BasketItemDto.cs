using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.dtos
{
    public class BasketItemDto
    {

        public int ProductId { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required string Category { get; set; }

        public string[]? Urls { get; set; }

        public int Stock { get; set; }
        public required string Brand { get; set; }

        public int Quantity { get; set; }


    }
}