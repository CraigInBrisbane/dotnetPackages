using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Handlers
{
    public class AddUserRoleRequest : IRequest<UserRoleAdditionResponse>
    {
        public int RoleId { get; set; }
        public Guid UserId { get; set; }
    }

    public class AddUserRoleRequestHandler : IRequestHandler<AddUserRoleRequest, UserRoleAdditionResponse>
    {
        private readonly ILogger<AuthenticateUserCommandHandler> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AddUserRoleRequestHandler(ILogger<AuthenticateUserCommandHandler> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        public async Task<UserRoleAdditionResponse> Handle(AddUserRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.AddRoleToUser(request.UserId, request.RoleId);
            return result;
        }
    }
}