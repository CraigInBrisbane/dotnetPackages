using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.User.Handlers;
using Domain.Requests;

namespace Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediatr;
        private readonly IMapper _mapper;
        public UserController(IMediator mediatr, IMapper mapper)
        {
            this._mediatr = mediatr;
            this._mapper = mapper;
        }
        
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns>Primary key of new user</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await this._mediatr.Send(_mapper.Map<RegisterUserRequest, RegisterUserQuery>(request));
            return Ok(result);
        }

        /// <summary>
        /// Gets a full list of users
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await this._mediatr.Send(new GetUsersQuery());
            return Ok(result);
        }

        [HttpPost, Route("{emailValidationId}/validate")]
        public async Task<IActionResult> ValidateEmail(Guid emailValidationId)
        {
            var result = await _mediatr.Send(new ValidateEmailCommand(emailValidationId));
            return Ok(result);
        }
    }
}
