using System;

namespace Domain.Responses
{
    public class AuthenticateUserResponse : Response
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
    }
}