﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ScreensController : ControllerBase
    {
        private readonly ScreensBusiness business;

        public ScreensController(ScreensBusiness business)
        {
            this.business = business;
        }

        /// <summary>
        /// Get all Screens created
        /// </summary>
        /// <returns>Return all Screens</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Screen>>> GetScreens()
            => Ok(await business.GetAll());

        /// <summary>
        /// Get all Screens created with paginattion
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <returns>Return all Screens</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("paginate/{page}/{limitPage}")]
        public async Task<ActionResult<IEnumerable<Screen>>> GetScreensPaginate(int page, int limitPage)
            => Ok(await business.GetAllPaginate(page, limitPage));

        /// <summary>
        /// Get total of registers created
        /// </summary>
        /// <returns>Return total of registers</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountRegisters()
        {
            int count = await business.CountActived();

            if (count > 0)
                return Ok(count);

            return NotFound();
        }

        /// <summary>
        /// Get a Screen by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Screen</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Screen>> GetScreen(int id)
        {
            Screen screen = await business.GetById(id);

            if (screen is null)
                return NotFound();

            return Ok(screen);
        }

        /// <summary>
        /// Get all Screen with same Size
        /// </summary>
        /// <param name="size"></param>
        /// <returns>Return Screens</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<Screen>>> GetBySize(string size)
        {
            IEnumerable<Screen> screens = await business.GetBySize(size);

            if (screens is null)
                return NotFound();

            return Ok(screens);
        }

        /// <summary>
        /// Get all Screens in a ComboBox (Id, Value)
        /// </summary>
        /// <returns>Return a Screens's ComboBox</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("comboBox")]
        public async Task<ActionResult<IEnumerable<ComboBoxViewModel>>> GetComboBox()
            => Ok(await business.GetComboBox());

        /// <summary>
        /// Create a Screen
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Size": "Extreme"
        ///     }
        /// </remarks>
        /// <param name="screen"></param>
        /// <returns>Return a Screen created</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        public async Task<ActionResult<Screen>> PostScreen(Screen screen)
        {
            try
            {
                screen = await business.Create(screen);

                return Ok(screen);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update a Screen by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "Size": "Extreme"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="screen"></param>
        /// <returns>Return a Screen updated</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScreen(int id, Screen screen)
        {
            if (id != screen.id)
                return BadRequest();

            if (await business.Update(screen))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete a Screen by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a Screen deleted</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            if (await business.DeleteById(id))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}