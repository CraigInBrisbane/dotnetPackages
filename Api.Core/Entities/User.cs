using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int CountryId { get; set; }
        public DateTimeOffset EmailValidatedOn { get; set; }
    }
}
