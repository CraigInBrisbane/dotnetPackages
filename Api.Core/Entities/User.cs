using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Core.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(50), Required]
        public string Firstname { get; set; }
        
        [MaxLength(50), Required]
        public string Surname { get; set; }
        
        public int CountryId { get; set; }
        
        public ICollection<UserEmail> UserEmails { get; set; }
        
    }
}
