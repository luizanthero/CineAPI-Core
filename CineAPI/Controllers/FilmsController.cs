using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineAPI.Business.Entities;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmsBusiness business;

        public FilmsController(FilmsBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all Films createds
        /// </summary>
        /// <returns>Return all Films</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get a Film by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Film</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            Film film = await business.GetById(id);

            if (film is null)
                return NotFound();

            return Ok(film);
        }

        /// <summary>
        /// Get a Film by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return a Film</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<ComboBoxViewModel>>> GetComboBoxName(string name)
        {
            var film = await business.GetComboBox(name);

            if (film is null)
                return NotFound();

            return Ok(film);
        }

        /// <summary>
        /// Create a Film
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         "Name": "Wonder Woman",
        ///         "ApiCode": "tt0451279"
        ///     }
        ///     
        /// </remarks>
        /// <param name="film"></param>
        /// <returns>Return a Film created</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(Film film)
        {
            try
            {
                film = await business.Create(film);
                return Ok(film);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}