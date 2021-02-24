using System;

namespace Domain.Responses
{
    public class ChangeUserEmailResponse : Response
    {
        public Guid ValidationId { get; set; }
    }
}