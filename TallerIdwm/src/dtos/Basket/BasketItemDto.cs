using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.dtos
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }


        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public required string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public required string Category { get; set; }

        public string[]? Urls { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public required string Brand { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa.")]
        public int Quantity { get; set; }


    }
}