using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public EmailDto Email { get; set; }
        public ICollection<RoleDto> Roles { get; set; }
    }
}