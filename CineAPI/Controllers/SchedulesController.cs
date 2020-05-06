using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineAPI.Business.Entities;
using CineAPI.Models;
using CineAPI.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly SchedulesBusiness business;

        public SchedulesController(SchedulesBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all Schedules created
        /// </summary>
        /// <returns>Return all Schedules</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get all Schedules created with pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <returns>Return all Schedules</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("paginate/{page}/{limitPage}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesPaginate(int page, int limitPage)
            => Ok(await business.GetAllPaginate(page, limitPage));

        /// <summary>
        /// Get total of registers created
        /// </summary>
        /// <returns>Return a total of registers</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountRegisters()
        {
            var count = await business.CountActived();

            if (count > 0)
                return Ok(count);

            return NotFound();
        }

        /// <summary>
        /// Get a Schedule by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Schedule</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetRoom(int id)
        {
            Schedule schedule = await business.GetById(id);

            if (schedule is null)
                return NotFound();

            return Ok(schedule);
        }

        /// <summary>
        /// Get a Schedule by Description
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Return a Schedule</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("description/{description}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetByDescription(string description)
        {
            IEnumerable<Schedule> schedules = await business.GetByDescription(description);

            if (schedules is null)
                return NotFound();

            return Ok(schedules);
        }

        /// <summary>
        /// Get all Schedules in a ComboBox (Id, Value)
        /// </summary>
        /// <returns>Return a Schedules's ComboBox</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        public async Task<ActionResult<IEnumerable<ComboBoxViewModel>>> GetComboBox()
            => Ok(await business.GetComboBox());

        /// <summary>
        /// Create a Schedule
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Description": "teste"
        ///     }
        /// </remarks>
        /// <param name="schedule"></param>
        /// <returns>Return a Schedule created</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        public async Task<ActionResult<Schedule>> PostSchedule(Schedule schedule)
        {
            try
            {
                schedule = await business.Create(schedule);

                return Ok(schedule);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update a Schedule by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Description": "teste"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="schedule"></param>
        /// <returns>Return a Schedule updated</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, Schedule schedule)
        {
            if (id != schedule.id)
                return BadRequest();

            if (await business.Update(schedule))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete a Schedule by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Schedule</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            if (await business.DeleteById(id))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}