using System.Collections.Generic;
using Api.Core.Contracts.DTOs;

namespace Api.Core.Contracts.Responses
{
    public class UserListResponse
    {
        public List<UserDto> Data { get; set; }
    }
}