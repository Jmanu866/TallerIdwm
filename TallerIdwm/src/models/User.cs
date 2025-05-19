using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TallerIdwm.src.models
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow; // Registration date
        public DateTime? LastAccess { get; set; } // Last login timestamp
        public bool IsActive { get; set; } = true; // Whether the user can log in
        public string? DeactivationReason { get; set; } // Admin reason for disabling the account
        public DateOnly? BirthDate { get; set; } // User's birth date
        // Navigation properties
        public ShippingAddress? ShippingAddress { get; set; }



    }



}