using System;
using System.Threading.Tasks;
using Domain.Requests;
using Domain.Responses;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticateUserResponse> Login(LoginRequest request);
        Task<UserRoleAdditionResponse> AddRoleToUser(Guid userId, int roleId);
    }
}