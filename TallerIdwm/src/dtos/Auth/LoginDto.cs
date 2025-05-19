using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.dtos
{
    public class LoginDto
    {
    
    public required string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public required string Password { get; set; }
    }
}