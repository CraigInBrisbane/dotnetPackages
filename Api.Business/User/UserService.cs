using Api.Business.Interfaces;
using Api.Business.User.Handlers;
using Api.Core.Contracts.Responses;
using Infrastructure.Database;
using System;
using System.Threading.Tasks;

namespace Api.Business.User
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _db;

        public UserService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<RegisterUserResponse> Register(RegisterUserQuery request)
        {
            //todo: add entity validation...

            // ???
            var id = Guid.NewGuid();

            _db.Add(new Core.Entities.User
            {
                CountryId = request.CountryId,
                Created = DateTimeOffset.Now,
                Email = request.Email,
                EmailValidatedOn = DateTimeOffset.Now,
                Firstname = request.Firstname,
                Id = id,
                Surname = request.Surname,
                Updated = null
            });

            _db.SaveChanges();

            return new RegisterUserResponse
            {
                Email = request.Email,
                UserId = id,
            };
        }
    }
}
