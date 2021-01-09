using Api.Business.Interfaces;
using Api.Business.User.Handlers;
using Api.Core.Contracts.Responses;
using System;
using System.Threading.Tasks;

namespace Api.Business.User
{
    public class UserService : IUserService
    {

        private readonly IDbContext _context;
        public UserService(IDbContext dbContext)
        {

        }
        public async Task<RegisterUserResponse> Register(RegisterUserQuery request)
        {
            // we need to save the user to a database, when we work out how to add EF Core here.
            return new RegisterUserResponse
            {
                Email = request.Email,
                UserId = Guid.NewGuid()
            };
        }
    }
}
