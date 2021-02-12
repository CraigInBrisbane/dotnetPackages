using System.Threading.Tasks;
using Application.Authentication.Handlers;
using Domain.Requests;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    
    [Route("api/token")]
    public class Token  : ControllerBase
    {
        private readonly IMediator _mediator;

        public Token(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _mediator.Send(new AuthenticateUserCommand
                {Username = request.Username, Password = request.Password});

            return Ok(result);
        }
        
    }
}