﻿using System;

namespace Domain.DTOs
{
    public class EmailDto
    {
        public string Current { get; set; }
        public bool CurrentIsValidated { get; set; }
        public string LastValidated { get; set; }
        public Guid? EmailValidationId { get; set; }
    }
}