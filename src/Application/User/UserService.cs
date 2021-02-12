using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.User.Handlers;
using Domain.DTOs;
using Domain.Responses;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.User
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserService> _log;

        public UserService(DatabaseContext context, ILogger<UserService> log)
        {
            _context = context;
            _log = log;
        }

        public async Task<RegisterUserResponse> Register(RegisterUserQuery request)
        {
            var id = Guid.NewGuid();

            var user = new Infrastructure.Database.Entities.User
            {
                CountryId = request.CountryId,
                Created = DateTimeOffset.Now,
                Firstname = request.Firstname,
                Id = id,
                Surname = request.Surname,
                Updated = null
            };

            var userEmail = new UserEmail
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                User = user,
                Created = DateTimeOffset.UtcNow,
            };

            await _context.Users.AddAsync(user);
            await _context.UserEmails.AddAsync(userEmail);

            await _context.SaveChangesAsync();

            return new RegisterUserResponse
            {
                Email = request.Email,
                UserId = id,
                EmailValidationId = userEmail.Id
            };
        }

        public async Task<UserListResponse> GetUsers()
        {
            
            var data = await _context.Users
                .Include(x => x.UserEmails)
                .Select(x => new UserDto
                {
                    Email = new EmailDto //TODO: This seems like we're doing 4 queries... can maybe be simplified?
                    {
                        Current = x.UserEmails.OrderByDescending(u=>u.Created).FirstOrDefault().Email,
                        CurrentIsValidated = x.UserEmails.OrderByDescending(u=>u.Created).FirstOrDefault().ValidatedDate.HasValue,
                        LastValidated = x.UserEmails.OrderByDescending(u=>u.Created).FirstOrDefault(y=>y.ValidatedDate.HasValue).Email,
                        EmailValidationId = x.UserEmails.OrderByDescending(u=>u.Created).FirstOrDefault(x=>x.ValidatedDate.HasValue == false).Id,
                    },
                    EmailValidated = x.UserEmails.OrderByDescending(y => y.Created).FirstOrDefault().ValidatedDate.HasValue, // TODO: Maybe this doesn't need to be done twice, and we can get the email stuff before in one call?
                    Firstname = x.Firstname,
                    Id = x.Id,
                    Surname = x.Surname
                }).ToListAsync();

            return new UserListResponse {Data = data};
        }

        public async Task<GenericResponse> ValidateEmail(Guid emailValidationId)
        {
            var emailValidation = await _context.UserEmails.SingleOrDefaultAsync(x => x.Id == emailValidationId);
            if (emailValidation is null)
            {
                _log.LogWarning("Attempt to validate invalid Email Validation Id {EmailValidationId}", emailValidationId);
                return new GenericResponse
                {
                    Message = "Invalid validation token",
                    Success = false,
                    ResponseCode = 404
                };
            }

            if (emailValidation.ValidatedDate.HasValue)
            {
                _log.LogWarning("Attempt to validate already validated Email with Validation Id {EmailValidationId}", emailValidationId);
                return new GenericResponse()
                {
                    Message = "Email is already validated",
                    Success = false,
                    ResponseCode = 400
                };
            }

            emailValidation.ValidatedDate = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();

            return new GenericResponse
            {
                Message = "Your email address has been validated",
                Success = true,
                ResponseCode = 200
            };
        }
    }
}