using System;
using System.Collections.Generic;
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
    public class UsersController : ControllerBase
    {
        private readonly UsersBusiness business;

        public UsersController(UsersBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all users created
        /// </summary>
        /// <returns>Return all users</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get all users created with paginattion
        /// </summary>
        /// <returns>Return all users</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("paginate/{page}/{limitPage}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<PaginationViewModel<User>>> GetAllPaginate(int page, int limitPage)
            => Ok(await business.GetAllPaginate(page, limitPage));

        /// <summary>
        /// Get total of registers created
        /// </summary>
        /// <returns>Return total of registers</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("count")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<int>> CountRegisters()
        {
            int count = await business.CountActived();

            if (count > 0)
                return Ok(count);

            return NotFound();
        }

        /// <summary>
        /// Get a User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a User</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<User>> GetUser(int id)
            => Ok(await business.GetById(id));

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
        public async Task<ActionResult<string>> Authenticate(UserAuthenticateViewModel user)
        {
            try
            {
                string token = await business.Authenticate(user.Username, user.Password);

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Username or Password is incorrect!");

                return Ok(token);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}