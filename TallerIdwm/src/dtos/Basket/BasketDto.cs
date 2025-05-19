using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.dtos
{
    public class BasketDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El ID del carrito es obligatorio.")]
        public required string BasketId { get; set; }

        [Required(ErrorMessage = "Debe haber al menos un ítem en el carrito.")]
        [MinLength(1, ErrorMessage = "El carrito debe contener al menos un producto.")]
        public List<BasketItemDto> Items { get; set; } = [];

    }
}