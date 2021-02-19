using Domain.DTOs;

namespace Application.Helpers
{
    public interface ITokenHelper
    {
        string GenerateToken(UserDto user);
    }
}