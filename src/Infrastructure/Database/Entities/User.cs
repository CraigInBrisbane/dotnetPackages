using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(50), Required]
        public string Firstname { get; set; }
        
        [MaxLength(50), Required]
        public string Surname { get; set; }
        
        public int CountryId { get; set; }

        [MaxLength(200), Required]
        public string Password { get; set; }

        
        // Navigation Properties
        public ICollection<UserEmail> UserEmails { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
        
        
    }
}
