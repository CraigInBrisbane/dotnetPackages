using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.User.Handlers
{
    public class GetUsersQuery : IRequest<UserListResponse>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, UserListResponse>
    {
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly IUserService _service;

        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<UserListResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get User List handler executed");
            var result = await _service.GetUsers();
            return result;
        }
    }
}