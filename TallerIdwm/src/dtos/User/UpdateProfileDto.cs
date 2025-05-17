using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.dtos
{
    public class UpdateProfileDto
    {
        public string FirtsName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
    }
}