using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Authentication.Handlers;
using Application.User.Handlers;
using Domain.Requests;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/users")]
    [ApiVersion("1.0")]
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
        /// <returns>Primary key of new user</returns>
        [HttpPost, MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await this._mediatr.Send(request.Adapt<RegisterUserQuery>());
            return Ok(result);
        }

        [HttpGet, Route("{id}"), Authorize, MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await this._mediatr.Send(new GetUserByIdQuery {Id = id});
            return Ok(result);
        }

        /// <summary>
        /// Gets a full list of users
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet, Authorize, MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await this._mediatr.Send(new GetUsersQuery());
            return Ok(result);
        }

        [HttpPost, Route("{emailValidationId}/validate"), MapToApiVersion("1.0")]
        public async Task<IActionResult> ValidateEmail(Guid emailValidationId)
        {
            var result = await _mediatr.Send(new ValidateEmailCommand(emailValidationId));
            return Ok(result);
        }

        [HttpPost, Route("{userId}/roles"), MapToApiVersion("1.0")]
        public async Task<IActionResult> AddRoleToUser(Guid userId, int roleId)
        {
            var result = await _mediatr.Send(new AddUserRoleRequest { UserId = userId, RoleId = roleId});
            return Ok(result);
        }

        [HttpPost, Route("{userId}/email"), MapToApiVersion("1.0"), Authorize]
        public async Task<IActionResult> ChangeEmail(Guid userId, string email)
        {
            // Need to check if this is the logged in user... get from Context? Token?
            var result = await _mediatr.Send(new ChangeUserEmailCommand {UserId = userId, Email = email});
            return Ok(result);
        }
    }
}