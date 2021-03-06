using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Helpers;
using Application.Interfaces;
using Application.User.Handlers;
using Domain.Requests;
using Domain.Responses;
using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Infrastructure.Helpers;
using Infrastructure.Providers.Clock;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger _logger;
        private readonly DatabaseContext _context;
        private readonly IMediator _mediator;
        private readonly ITokenHelper _tokenHelper;
        private readonly IClockProvider _clock;

        public AuthenticationService(ILogger<AuthenticationService> logger, DatabaseContext context, IMediator mediator,
            ITokenHelper tokenHelper, IClockProvider clock)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
            _tokenHelper = tokenHelper;
            _clock = clock;
        }

        public async Task<AuthenticateUserResponse> Login(LoginRequest request)
        {
            _logger.LogInformation($"Login attempt for {request.Username}");

            // Get the user that owns this email address, if any.
            var matchedEmail = await _context
                .UserEmails
                .Include(x => x.User)
                .OrderByDescending(x=>x.Created)
                .FirstOrDefaultAsync(x => x.Email == request.Username);

            if (matchedEmail == null)
                return new AuthenticateUserResponse
                {
                    Message = "Invalid Username/Password",
                    Success = false,
                    ResponseCode = 404
                };
            
            // Validate it's the latest validated email.
            var emails = await _context
                .UserEmails
                .Where(x => x.User == matchedEmail.User && x.ValidatedDate.HasValue)
                .OrderByDescending(x => x.ValidatedDate)
                .ToListAsync();

            if (emails != null && emails.FirstOrDefault()?.Email != request.Username)
                return new AuthenticateUserResponse
                {
                    Message = "Email is obsolete",
                    ResponseCode = 401,
                    Success = false
                };
            
            // Encrypt the incoming password so we can check against the hashed one in database.
            var encryptedPassword =
                EncryptionHelper.GenerateSaltedHash(request.Password, matchedEmail.User.Id.ToString());

            var user = await _context
                .Users
                .Where(x => x.Id == matchedEmail.User.Id
                            && x.Password == encryptedPassword)
                .SingleOrDefaultAsync();

            _logger.LogInformation($"Login Success for {request.Username}");


            var userDetails = await _mediator.Send(new GetUserByIdQuery {Id = user.Id});

            var token = _tokenHelper.GenerateToken(userDetails.Data);

            return new AuthenticateUserResponse
            {
                Firstname = user.Firstname,
                Id = user.Id,
                Surname = user.Surname,
                Token = token,
                Message = "Login Success",
                Success = true,
                ResponseCode = 200,
            };
        }

        public async Task<UserRoleAdditionResponse> AddRoleToUser(Guid userId, int roleId)
        {
            _logger.LogInformation("Adding role {roleId} to user {userId}");
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return new UserRoleAdditionResponse
                {
                    Message = "User not found",
                    Success = false,
                    ResponseCode = 404
                };
            }

            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Id == roleId);
            if (role == null)
            {
                return new UserRoleAdditionResponse
                {
                    Message = "Role not found",
                    Success = false,
                    ResponseCode = 404
                };
            }

            if (await _context.UserRoles.AnyAsync(x => x.User == user && x.Role == role))
                return new UserRoleAdditionResponse
                {
                    Message = $"User already has the role of {role.Name}",
                    Success = false,
                    ResponseCode = 400
                };

            var newItem = new UserRole
            {
                Created = _clock.UtcNow(),
                Role = role,
                User = user
            };
            await _context.UserRoles.AddAsync(newItem);

            await _context.SaveChangesAsync();

            return new UserRoleAdditionResponse
            {
                Id = newItem.Id,
                Message = "Role added successfully",
                Success = true,
                ResponseCode = 201
            };
        }
    }
}