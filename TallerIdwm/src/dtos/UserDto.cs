using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerIdwm.src.models;



namespace TallerIdwm.src.dtos
{
    public class UserDto
    {

        public string FirstName { get; set; } 

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Phone { get; set; }

        public string? Street { get; set; }

        public string? Number { get; set; }

        public string? Commune { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }
    }
}