using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Core.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        [MaxLength(50, ErrorMessage = "Email is required"), Required]
        public string Email { get; set; }
        [MaxLength(50), Required]
        public string Firstname { get; set; }
        [MaxLength(50), Required]
        public string Surname { get; set; }
        public int CountryId { get; set; }
        public DateTimeOffset? EmailValidatedOn { get; set; }
    }
}
