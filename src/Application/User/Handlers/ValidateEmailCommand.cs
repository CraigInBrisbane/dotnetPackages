using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.User.Handlers
{
    public class ValidateEmailCommand : IRequest<GenericResponse>
    {
        public ValidateEmailCommand(Guid id)
        {
            EmailValidationId = id;
        }
        public Guid EmailValidationId { get; set; }
    }

    public class ValidateEmailCommandHandler : IRequestHandler<ValidateEmailCommand, GenericResponse>
    {

        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public ValidateEmailCommandHandler(ILogger<ValidateEmailCommandHandler> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<GenericResponse> Handle(ValidateEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Email validation handler being executed with Id {EmailValidationId}", request.EmailValidationId);
            var result = await _userService.ValidateEmail(request.EmailValidationId);
            return result;
        }
    }
}