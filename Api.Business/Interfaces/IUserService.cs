using System;
using Api.Business.User.Handlers;
using Api.Core.Contracts.Responses;
using System.Threading.Tasks;

namespace Api.Business.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserResponse> Register(RegisterUserQuery request);
        Task<UserListResponse> GetUsers();
        Task<bool> ValidateEmail(Guid emailValidationId);
    }
}
