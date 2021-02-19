using System;

namespace Domain.Responses
{
    public class AuthenticateUserResponse : Response
    {
        public string Token { get; set; }
    }
}