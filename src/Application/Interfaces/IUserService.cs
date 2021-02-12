using System;
using System.Threading.Tasks;
using Application.User.Handlers;
using Domain.Responses;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserResponse> Register(RegisterUserQuery request);
        Task<UserListResponse> GetUsers();
        Task<GenericResponse> ValidateEmail(Guid emailValidationId);
    }
}
