using Api.Business.Interfaces;
using Api.Core.Contracts.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Business.User.Handlers
{

    public class RegisterUserQuery : IRequest<RegisterUserResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int CountryId { get; set; }

    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserQuery, RegisterUserResponse>
    {

        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler executing for email {Email}", request.Email);
            var result = await _userService.Register(request);
            return result;
        }
    }


}
