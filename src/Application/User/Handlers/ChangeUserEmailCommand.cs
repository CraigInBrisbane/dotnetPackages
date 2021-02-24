using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.User.Handlers
{
    public class ChangeUserEmailCommand : IRequest<ChangeUserEmailResponse>
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }

    public class ChangeUserEmailCommandHandler : IRequestHandler<ChangeUserEmailCommand, ChangeUserEmailResponse>
    {
        private readonly ILogger<ChangeUserEmailCommandHandler> _logger;
        private readonly IUserService _service;

        public ChangeUserEmailCommandHandler(ILogger<ChangeUserEmailCommandHandler> logger, IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<ChangeUserEmailResponse> Handle(ChangeUserEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.ChangeEmail(request.UserId, request.Email);
            return result;
        }
    }
}