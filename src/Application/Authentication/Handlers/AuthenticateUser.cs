using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Requests;
using Domain.Responses;
using Infrastructure.Providers.Encryption;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Handlers
{

    public class AuthenticateUserCommand : IRequest<AuthenticateUserResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResponse>
    {
        private readonly ILogger<AuthenticateUserCommandHandler> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateUserCommandHandler(ILogger<AuthenticateUserCommandHandler> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        public async Task<AuthenticateUserResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.Login(new LoginRequest{Username = request.Username, Password = request.Password});

            if (result.Success)
            {
                
            }
            
            return result;
        }
    }
    
    public class AuthenticateUser
    {
        
    }
}