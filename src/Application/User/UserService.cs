using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.User.Handlers;
using Domain.DTOs;
using Domain.Responses;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Infrastructure.Helpers;
using Infrastructure.Providers.Clock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.User
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserService> _log;
        private readonly IClockProvider _clock;

        public UserService(DatabaseContext context, ILogger<UserService> log, IClockProvider clock)
        {
            _context = context;
            _log = log;
            _clock = clock;
        }

        public async Task<RegisterUserResponse> Register(RegisterUserQuery request)
        {
            var id = Guid.NewGuid();

            var encryptedPassword = EncryptionHelper.GenerateSaltedHash(request.Password, id.ToString());
            
            var user = new Infrastructure.Database.Entities.User
            {
                CountryId = request.CountryId,
                Created = _clock.UtcNow(),
                Firstname = request.Firstname,
                Id = id,
                Surname = request.Surname,
                Updated = null,
                Password = encryptedPassword
            };


            var userRole = new UserRole
            {
                User = user,
                Role = await _context.Roles.SingleAsync(x => x.Name == "User"),
                Created = _clock.UtcNow()
            };

            var userEmail = new UserEmail
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                User = user,
                Created = _clock.UtcNow()
            };

            await _context.Users.AddAsync(user);
            await _context.UserEmails.AddAsync(userEmail);
            await _context.UserRoles.AddAsync(userRole);

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
                            .FirstOrDefault(u => u.ValidatedDate.HasValue == false).Id,
                    },
                    Roles = x.UserRoles.Select(r=> new RoleDto
                    {
                        Id = r.Role.Id,
                        Name = r.Role.Name
                    }).ToList(),
                    Firstname = x.Firstname,
                    Id = x.Id,
                    Surname = x.Surname
                }).ToListAsync();

            return new UserListResponse
                {Data = data, Message = $"Returning {data.Count} Users", Success = true, ResponseCode = 200};
        }

        public async Task<UserResponse> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Empty UserId");
            
            var data = await _context.Users
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
                            .FirstOrDefault(u => u.ValidatedDate.HasValue == false).Id,
                    },
                    Roles = x.UserRoles.Select(r => new RoleDto
                    {
                        Id = r.Role.Id,
                        Name = r.Role.Name
                    }).ToList(),
                    Firstname = x.Firstname,
                    Id = x.Id,
                    Surname = x.Surname
                })
                .SingleOrDefaultAsync(x => x.Id == id);

            if (data != null)
                return new UserResponse
                {
                    Data = data
                };
            _log.LogWarning("Attempt to get user with Id {Id} failed");
            
            return null;

        }

        public async Task<ChangeUserEmailResponse> ChangeEmail(Guid userId, string email)
        {
            if (userId == Guid.Empty || email == string.Empty)
                throw new ArgumentException("Email is not valid");

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return new ChangeUserEmailResponse
                {
                    Success = false,
                    Message = "Invalid user id",
                    ResponseCode = 404
                };


            var userEmail = new UserEmail
            {
                Created = _clock.UtcNow(),
                Email = email,
                User = user
            };

            await _context.UserEmails.AddAsync(userEmail);
            await _context.SaveChangesAsync();

            return new ChangeUserEmailResponse
            {
                Message = "Email updated. Expecting validation",
                Success = true,
                ResponseCode = 201,
                ValidationId = userEmail.Id
            };
        }

        public async Task<GenericResponse> ValidateEmail(Guid emailValidationId)
        {
            if (emailValidationId == Guid.Empty)
                throw new ArgumentException("Empty Email Validation");

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

            emailValidation.ValidatedDate = _clock.UtcNow();
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