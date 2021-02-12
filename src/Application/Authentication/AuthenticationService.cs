using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Requests;
using Domain.Responses;
using Infrastructure.Database;
using Infrastructure.Providers.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly ILogger _logger;
        private readonly DatabaseContext _context;
        
        public AuthenticationService(ILogger<AuthenticationService> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public async Task<AuthenticateUserResponse> Login(LoginRequest request)
        {
            _logger.LogInformation($"Login attempt for {request.Username}");
            
            // Does the email exist? We need the Id to match the password.
            var matchedEmail = await _context
                .UserEmails
                .Include(x=>x.User)
                .OrderByDescending(x => x.Created)
                .SingleOrDefaultAsync(x => x.Email == request.Username);
                

            if (matchedEmail is not null)
            {
                var encryptedPassword =
                    EncryptionProvider.GenerateSaltedHash(request.Password, matchedEmail.User.Id.ToString());

                var user = await _context
                    .Users
                    .SingleOrDefaultAsync(x => x.Id == matchedEmail.User.Id
                                               && x.Password == encryptedPassword
                    );

                if (user != null)
                {

                    if (matchedEmail.ValidatedDate.HasValue == false)
                    {
                        _logger.LogInformation("Login details match by email has not yet been validated");
                        return new AuthenticateUserResponse
                        {
                            Message = "Email has not been validated yet",
                            ResponseCode = 400,
                            Success = false
                        };
                    }
                    
                    _logger.LogInformation($"Login Success for {request.Username}");
                    return new AuthenticateUserResponse
                    {
                        Firstname = user.Firstname,
                        Id = user.Id,
                        Surname = user.Surname,
                        Message = "Login Success",
                        Success = true,
                        ResponseCode = 200
                    };
                }
            }

            _logger.LogWarning($"Login Failed for {request.Username}");

            return new AuthenticateUserResponse
            {
                Message = "Invalid Username/Password",
                ResponseCode = 400
            };
        }
    }
}