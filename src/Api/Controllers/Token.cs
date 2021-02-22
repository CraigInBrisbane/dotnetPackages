using System.Threading.Tasks;
using Application.Authentication.Handlers;
using Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    
    [Route("api/token")]
    [ApiVersion("1.0")]
    public class Token  : ControllerBase
    {
        private readonly IMediator _mediator;

        public Token(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _mediator.Send(new AuthenticateUserCommand
                {Username = request.Username, Password = request.Password});
            
            return Ok(result);
        }
        
    }
}