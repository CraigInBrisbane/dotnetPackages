using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Database.Entities
{
    public class UserEmail : BaseEntity
    {
        [Required]
        public User User { get; set; }
        
        [MaxLength(100)]
        public string Email { get; set; }
        
        public DateTimeOffset? ValidatedDate { get; set; }
        
    }
}