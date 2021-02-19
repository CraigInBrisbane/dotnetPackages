using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.User.Handlers;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/user")]
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
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await this._mediatr.Send(new RegisterUserQuery
            {
                Email = request.Email, Firstname = request.Firstname, Password = request.Password,
                Surname = request.Surname, CountryId = request.CountryId
            });
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await this._mediatr.Send(new GetUserByIdQuery {Id = id});
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