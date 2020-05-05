using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineAPI.Business.Entities;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly RoomTypesBusiness business;

        public RoomTypesController(RoomTypesBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all Room Types
        /// </summary>
        /// <returns>Return all Room Types</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomType>>> GetRoomTypes()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get all Room Type with pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <returns>Return all Room Types</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("paginate/{page}/{limitPage}")]
        public async Task<ActionResult<IEnumerable<RoomType>>> GetRoomTypesPaginate(int page, int limitPage)
            => Ok(await business.GetAllPaginate(page, limitPage));

        /// <summary>
        /// Get a Room Type by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Room Type</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomType>> GetRoomType(int id)
        {
            RoomType roomType = await business.GetById(id);

            if (roomType is null)
                return NotFound();

            return Ok(roomType);
        }

        /// <summary>
        /// Get Room Types with same Description
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Return Room Types</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("description/{description}")]
        public async Task<ActionResult<IEnumerable<RoomType>>> GetByDescription(string description)
        {
            IEnumerable<RoomType> roomTypes = await business.GetByDescription(description);

            if (roomTypes is null)
                return NotFound();

            return Ok(roomTypes);
        }

        /// <summary>
        /// Get all Room Types in a ComboBox (Id, Value)
        /// </summary>
        /// <returns>Return a Room Types's ComboBox</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("comboBox")]
        public async Task<ActionResult<IEnumerable<ComboBoxViewModel>>> GetComboBox()
            => Ok(await business.GetComboBox());

        /// <summary>
        /// Create a Room Type
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Description": "XD 3D"
        ///     }
        /// </remarks>
        /// <param name="roomType"></param>
        /// <returns>Return a Room Type created</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        public async Task<ActionResult<RoomType>> PostRoomType(RoomType roomType)
        {
            try
            {
                roomType = await business.Create(roomType);
                return Ok(roomType);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update a Room Type by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Description": "XD 3D"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="roomType"></param>
        /// <returns>Return a Room Type updated</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomType(int id, RoomType roomType)
        {
            if (id != roomType.id)
                return BadRequest();

            if (await business.Update(roomType))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete a Room Type by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Room Type deleted</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            if (await business.DeleteById(id))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}