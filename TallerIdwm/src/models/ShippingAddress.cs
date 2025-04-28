using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.models
{
    public class ShippingAddress
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Commune { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public int UserId { get; set; } // Foreign key to User

        public User User { get; set; } = null!; // Navigation property to User
    }
}