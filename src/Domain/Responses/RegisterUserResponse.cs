﻿using System;

namespace Domain.Responses
{
    public class RegisterUserResponse : Response
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public Guid EmailValidationId { get; set; }
    }
}
