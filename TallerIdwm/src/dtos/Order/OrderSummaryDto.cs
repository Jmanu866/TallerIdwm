using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.dtos.Order
{
    public class OrderSummaryDto
    {
         public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Total { get; set; }
    }
}