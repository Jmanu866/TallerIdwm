using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerIdwm.src.models
{
    public class ShippingAddress
    {
        public int Id { get; set; }

        public required string Street { get; set; }

        public required string Number { get; set; }

        public required string Commune { get; set; }

        public required string Region { get; set; }

        public required string PostalCode { get; set; }

        public string UserId { get; set; } = null!;// Foreign key to User

        public User User { get; set; } = null!; // Navigation property to User
    }
}