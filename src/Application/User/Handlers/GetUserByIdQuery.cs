using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Responses;
using MediatR;

namespace Application.User.Handlers
{
    public class GetUserByIdQuery: IRequest<UserResponse>
    {
        public Guid Id { get; set; }
    }
    

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IUserService _userService;

        public GetUserByIdQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _userService.GetUserById(request.Id);
            return result;
        }
    }
    
}