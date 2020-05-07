using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CineAPI.Business.Entities;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace CineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomsBusiness business;

        public RoomsController(RoomsBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all Rooms created
        /// </summary>
        /// <returns>Return all Rooms</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get all Rooms created with pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <returns>Return all Rooms</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("paginate/{page}/{limitPage}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsPaginate(int page, int limitPage)
            => Ok(await business.GetAllPaginate(page, limitPage));

        /// <summary>
        /// Get all Rooms details created 
        /// </summary>
        /// <returns>Return all Rooms</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("details")]
        public async Task<ActionResult<IEnumerable<RoomDetailsViewModel>>> GetRoomsDetails()
            => Ok(await business.GetAllDetails());

        /// <summary>
        /// Get a Room details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Room</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("details/{id}")]
        public async Task<ActionResult<IEnumerable<RoomDetailsViewModel>>> GetRoomDetails(int id)
            => Ok(await business.GetDetails(id));

        /// <summary>
        /// Get total of registers created
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountRegisters()
        {
            var count = await business.CountActived();

            if (count > 0)
                return Ok(count);

            return NotFound();
        }

        /// <summary>
        /// Get a Room by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Room</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            Room room = await business.GetById(id);

            if (room is null)
                return NotFound();

            return Ok(room);
        }

        /// <summary>
        /// Get a Room by Room Type Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Room</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("roomType/{id}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetByRoomType(int id)
        {
            IEnumerable<Room> rooms = await business.GetByRoomType(id);

            if (rooms is null)
                return NotFound();

            return Ok(rooms);
        }

        /// <summary>
        /// Get a Room by Screen Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Screen</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("screen/{id}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetByScreen(int id)
        {
            IEnumerable<Room> rooms = await business.GetByScreen(id);

            if (rooms is null)
                return NotFound();

            return Ok(rooms);
        }

        /// <summary>
        /// Get Rooms with same Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return Rooms</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetByName(string name)
        {
            IEnumerable<Room> rooms = await business.GetByName(name);

            if (rooms is null)
                return NotFound();

            return Ok(rooms);
        }

        /// <summary>
        /// Get all Rooms in a ComboBox (Id, Value)
        /// </summary>
        /// <returns>Return a Rooms's ComboBox</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("comboBox")]
        public async Task<ActionResult<IEnumerable<ComboBoxViewModel>>> GetComboBox()
            => Ok(await business.GetComboBox());

        /// <summary>
        /// Create a Room
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "RoomTypeId": 1,
        ///         "ScreenId": 1,
        ///         "Name": "teste"
        ///     }
        /// </remarks>
        /// <param name="room"></param>
        /// <returns>Return a Room created</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            try
            {
                room = await business.Create(room);

                return Ok(room);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update a Room by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "RoomTypeId": 1,
        ///         "ScreenId": 1,
        ///         "Name": "teste"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="room"></param>
        /// <returns>Return a Room updated</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.id)
                return BadRequest();

            if (await business.Update(room))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete a Room by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Room deleted</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (await business.DeleteById(id))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}