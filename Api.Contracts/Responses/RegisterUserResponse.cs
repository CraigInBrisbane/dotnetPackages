using System;

namespace Api.Core.Contracts.Responses
{
    public class RegisterUserResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        
        public Guid EmailValidationId { get; set; }
    }
}
