using Api.Business.User.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediatr;
        public UserController(IMediator mediatr)
        {
            this._mediatr = mediatr;
        }
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserQuery request)
        {
            var result = await this._mediatr.Send(request);
            return Ok(result);
        }
    }
}
