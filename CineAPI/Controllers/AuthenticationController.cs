using System;
using System.Threading.Tasks;
using CineAPI.Business.Entities;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UsersBusiness business;

        public AuthenticationController(UsersBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Register a User
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Username": "teste",
        ///         "Email": "teste@teste.com",
        ///         "Password": "teste"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>Return a register user</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Register(UserRegisterViewModel user)
        {
            try
            {
                User result = await business.Register(new User
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password
                });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Authenticate a User
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Username": "teste",
        ///         "Password": "teste"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>Return a Valid Token</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<UserAuthenticateTokenViewModel>> Authenticate(UserAuthenticateViewModel user)
        {
            try
            {
                var token = await business.Authenticate(user.Username, user.Password);

                if (string.IsNullOrEmpty(token.Token))
                    return BadRequest(new { message = "Username or Password is incorrect!" });

                return Ok(token);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}