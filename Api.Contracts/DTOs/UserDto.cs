using System;

namespace Api.Core.Contracts.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public EmailDto Email { get; set; }
        public bool EmailValidated { get; set; }
    }
}