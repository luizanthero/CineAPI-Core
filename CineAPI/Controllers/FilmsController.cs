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
    [Authorize]
    public class FilmsController : ControllerBase
    {
        private readonly FilmsBusiness business;

        public FilmsController(FilmsBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all Films created
        /// </summary>
        /// <returns>Return all Films</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get all Films created with pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <returns>Return all Films</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("paginate/{page}/{limitPage}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilmsPaginate(int page, int limitPage)
            => Ok(await business.GetAllPaginate(page, limitPage));

        /// <summary>
        /// Get all Films details created 
        /// </summary>
        /// <returns>Return all Films</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("details")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<FilmDetailsViewModel>>> GetFilmsDetails()
            => Ok(await business.GetAllDetails());

        /// <summary>
        /// Get a Film details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Film</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("details/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FilmDetailsViewModel>> GetFilmDetails(int id)
            => Ok(await business.GetDetails(id));

        /// <summary>
        /// Return total of Registers created
        /// </summary>
        /// <returns>Return total of Registers</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("count")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<int>> CountRegisters()
        {
            var count = await business.CountActived();

            if (count > 0)
                return Ok(count);

            return NotFound();
        }

        /// <summary>
        /// Get a Film by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Film</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            Film film = await business.GetById(id);

            if (film is null)
                return NotFound();

            return Ok(film);
        }

        /// <summary>
        /// Get Films with same Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return Films</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("name/{name}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Film>>> GetByName(string name)
        {
            IEnumerable<Film> films = await business.GetByName(name);

            if (films is null)
                return NotFound();

            return Ok(films);
        }

        /// <summary>
        /// Get a Film by ApiCode
        /// </summary>
        /// <param name="apiCode"></param>
        /// <returns>Return a Film</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("apiCode/{apiCode}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<Film>> GetByApiCode(string apiCode)
        {
            Film film = await business.GetByApiCode(apiCode);

            if (film is null)
                return NotFound();

            return Ok(film);
        }

        /// <summary>
        /// Get all Films in a ComboBox (Id, Value)
        /// </summary>
        /// <returns>Return a Films's ComboBox</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("comboBox")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<ComboBoxViewModel>>> GetComboBox()
        {
            var films = await business.GetComboBox();

            if (films is null)
                return NotFound();

            return Ok(films);
        }

        /// <summary>
        /// Create a Film
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         "name": "Harry Potter and the Deathly Hallows: Part 2",
        ///         "apiCode": "tt1201607",
        ///         "year": "2011",
        ///         "released": "15 Jul 2011",
        ///         "runtime": "130 min",
        ///         "genre": "Adventure, Drama, Fantasy, Mystery",
        ///         "director": "David Yates",
        ///         "writer": "Steve Kloves (screenplay), J.K. Rowling (novel)",
        ///         "actors": "Ralph Fiennes, Michael Gambon, Alan Rickman, Daniel Radcliffe",
        ///         "plot": "Harry, Ron, and Hermione search for Voldemort's remaining Horcruxes in their effort to destroy the Dark Lord as the final battle rages on at Hogwarts.",
        ///         "language": "English",
        ///         "country": "UK, USA",
        ///         "awards": "Nominated for 3 Oscars. Another 45 wins. 91 nominations.",
        ///         "poster": "https://m.media-amazon.com/images/M/MV5BMjIyZGU4YzUtNDkzYi00ZDRhLTljYzctYTMxMDQ4M2E0Y2YxXkEyXkFqcGdeQXVyNTIzOTk5ODM@._V1_SX300.jpg",
        ///         "type": "movie",
        ///         "production": "Warner Bros. Pictures",
        ///         "website": "N/A"
        ///     }
        ///     
        /// </remarks>
        /// <param name="film"></param>
        /// <returns>Return a Film created</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [Authorize(Roles = "Post")]
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

        /// <summary>
        /// Update a Film by Id
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
        /// <param name="id"></param>
        /// <param name="film"></param>
        /// <returns>Return a Film updated</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Put")]
        public async Task<IActionResult> PutFilm(int id, Film film)
        {
            if (id != film.id)
                return BadRequest();

            if (await business.Update(film))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete a Film by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Film deleted</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Delete")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            if (await business.DeleteById(id))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}