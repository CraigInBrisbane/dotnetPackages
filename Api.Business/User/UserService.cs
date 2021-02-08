using Api.Business.Interfaces;
using Api.Business.User.Handlers;
using Api.Core.Contracts.Responses;
using Infrastructure.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Core.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Api.Business.User
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<RegisterUserResponse> Register(RegisterUserQuery request)
        {
            var id = Guid.NewGuid();

            _context.Add(new Core.Entities.User
            {
                CountryId = request.CountryId,
                Created = DateTimeOffset.Now,
                Email = request.Email,
                EmailValidatedOn = null,
                Firstname = request.Firstname,
                Id = id,
                Surname = request.Surname,
                Updated = null
            });

            await _context.SaveChangesAsync();

            return new RegisterUserResponse
            {
                Email = request.Email,
                UserId = id,
            };
        }

        public async Task<UserListResponse> GetUsers()
        {
            var data = await _context.Users.Select(x => new UserDto
            {
                Email = x.Email,
                Firstname = x.Firstname,
                Id = x.Id,
                Surname = x.Surname
            }).ToListAsync();

            return new UserListResponse {Data = data};
        }
    }
}
