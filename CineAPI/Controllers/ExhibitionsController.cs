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
    public class ExhibitionsController : ControllerBase
    {
        private readonly ExhibitionsBusiness business;

        public ExhibitionsController(ExhibitionsBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all Exhibitions created
        /// </summary>
        /// <returns>Return all exhibitions</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Exhibition>>> GetExhibitions()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get all Exhibitons created with pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <returns>Return all exhibitions</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("paginate/{page}/{limitPage}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Exhibition>>> GetExhibitiosPaginate(int page, int limitPage)
            => Ok(await business.GetAllPaginate(page, limitPage));

        /// <summary>
        /// Get all Exhibitions details created 
        /// </summary>
        /// <returns>Return all Exhibitons</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("details")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<ExhibitionDetailsViewModel>>> GetExhibitionsDetails()
            => Ok(await business.GetAllDetails());

        /// <summary>
        /// Get a Exhibition details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Exhibiton</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("details/{id}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<ExhibitionDetailsViewModel>>> GetExhibitionsDetailsById(int id)
            => Ok(await business.GetDetails(id));

        /// <summary>
        /// Return total of registers created
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
        /// Get a Exhibition by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Exhibition</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<Exhibition>> GetExhibition(int id)
        {
            Exhibition exhibition = await business.GetById(id);

            if (exhibition is null)
                return NotFound();

            return Ok(exhibition);
        }

        /// <summary>
        /// Get a Exhibition by Film Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Exhibition</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("film/{id}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Exhibition>>> GetByFilm(int id)
        {
            IEnumerable<Exhibition> exhibitions = await business.GetByFilm(id);

            if (exhibitions is null)
                return NotFound();

            return Ok(exhibitions);
        }

        /// <summary>
        /// Get a Exhibition by Room Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Exhibition</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("room/{id}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Exhibition>>> GetByRoom(int id)
        {
            IEnumerable<Exhibition> exhibitions = await business.GetByRoom(id);

            if (exhibitions is null)
                return NotFound();

            return Ok(exhibitions);
        }

        /// <summary>
        /// Get a Exhibition by Schedule Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Exhibition</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("schedule/{id}")]
        [Authorize(Roles = "Get")]
        public async Task<ActionResult<IEnumerable<Exhibition>>> GetBySchedule(int id)
        {
            IEnumerable<Exhibition> exhibitions = await business.GetBySchedule(id);

            if (exhibitions is null)
                return NotFound();

            return Ok(exhibitions);
        }

        /// <summary>
        /// Create a Exhibition
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "FilmId": 1,
        ///         "RoomId": 1,
        ///         "Schedule": 1
        ///     }
        /// </remarks>
        /// <param name="exhibition"></param>
        /// <returns>Return a Exhibition created</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [Authorize(Roles = "Post")]
        public async Task<ActionResult<Exhibition>> PostExhibition(Exhibition exhibition)
        {
            try
            {
                exhibition = await business.Create(exhibition);
                return Ok(exhibition);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update a Exhibition by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "FilmId": 1,
        ///         "RoomId": 1,
        ///         "ScheduleId": 1
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="exhibition"></param>
        /// <returns>Return a Exhibition updated</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Put")]
        public async Task<IActionResult> PutExhibition(int id, Exhibition exhibition)
        {
            if (id != exhibition.id)
                return BadRequest();

            if (await business.Update(exhibition))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete a Exhibition by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Exhibition deleted</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Delete")]
        public async Task<IActionResult> DeleteExhibition(int id)
        {
            if (await business.DeleteById(id))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}