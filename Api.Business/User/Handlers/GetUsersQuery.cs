using System.Threading;
using System.Threading.Tasks;
using Api.Business.Interfaces;
using Api.Core.Contracts.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Api.Business.User.Handlers
{
    public class GetUsersQuery: IRequest<UserListResponse>
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