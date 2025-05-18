using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.dtos
{
    public class UpdateProfileDto
    {
      [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public DateOnly? BirthDate { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Email { get; set; }
    }
}