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
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UsersBusiness business;

        public UsersController(UsersBusiness business)
        {
            this.business = business;
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
        public async Task<ActionResult<string>> Authenticate(UserAuthenticate user)
        {
            try
            {
                string token = await business.Authenticate(user.Username, user.Password);

                if (token is null)
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