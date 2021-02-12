using System.Collections.Generic;
using Domain.DTOs;

namespace Domain.Responses
{
    public class UserListResponse
    {
        public List<UserDto> Data { get; set; }
    }
}