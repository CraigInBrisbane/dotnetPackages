using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.User.Handlers;
using Domain.DTOs;
using Domain.Responses;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Infrastructure.Providers.DateTime;
using Infrastructure.Providers.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.User
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserService> _log;
        private readonly IDateTimeProvider _dateTime;

        public UserService(DatabaseContext context, ILogger<UserService> log, IDateTimeProvider dateTime)
        {
            _context = context;
            _log = log;
            _dateTime = dateTime;
        }

        public async Task<RegisterUserResponse> Register(RegisterUserQuery request)
        {
            var id = Guid.NewGuid();

            var encryptedPassword = EncryptionProvider.GenerateSaltedHash(request.Password, id.ToString());
            
            var user = new Infrastructure.Database.Entities.User
            {
                CountryId = request.CountryId,
                Created = _dateTime.UtcNow(),
                Firstname = request.Firstname,
                Id = id,
                Surname = request.Surname,
                Updated = null,
                Password = encryptedPassword
            };

            var userEmail = new UserEmail
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                User = user,
                Created = _dateTime.UtcNow()
            };

            await _context.Users.AddAsync(user);
            await _context.UserEmails.AddAsync(userEmail);

            await _context.SaveChangesAsync();

            return new RegisterUserResponse
            {
                Email = request.Email,
                UserId = id,
                EmailValidationId = userEmail.Id,
                Message = "User registered successfully",
                Success = true,
                ResponseCode = 201
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
                        Current = x.UserEmails.OrderByDescending(u => u.Created).FirstOrDefault().Email,
                        CurrentIsValidated = x.UserEmails.OrderByDescending(u => u.Created).FirstOrDefault()
                            .ValidatedDate.HasValue,
                        LastValidated = x.UserEmails.OrderByDescending(u => u.Created)
                            .FirstOrDefault(y => y.ValidatedDate.HasValue).Email,
                        EmailValidationId = x.UserEmails.OrderByDescending(u => u.Created)
                            .FirstOrDefault(x => x.ValidatedDate.HasValue == false).Id,
                    },
                    Firstname = x.Firstname,
                    Id = x.Id,
                    Surname = x.Surname
                }).ToListAsync();

            return new UserListResponse
                {Data = data, Message = $"Returning {data.Count} Users", Success = true, ResponseCode = 200};
        }

        public async Task<GenericResponse> ValidateEmail(Guid emailValidationId)
        {
            var emailValidation = await _context.UserEmails.SingleOrDefaultAsync(x => x.Id == emailValidationId);
            if (emailValidation is null)
            {
                _log.LogWarning("Attempt to validate invalid Email Validation Id {EmailValidationId}",
                    emailValidationId);
                return new GenericResponse
                {
                    Message = "Invalid validation token",
                    Success = false,
                    ResponseCode = 404
                };
            }

            if (emailValidation.ValidatedDate.HasValue)
            {
                _log.LogWarning("Attempt to validate already validated Email with Validation Id {EmailValidationId}",
                    emailValidationId);
                return new GenericResponse()

                {
                    Message = "Email is already validated",
                    Success = false,
                    ResponseCode = 400
                };
            }

            emailValidation.ValidatedDate = _dateTime.UtcNow();
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