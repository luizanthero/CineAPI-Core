using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineAPI.Business.Entities;
using CineAPI.Models;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
            => Ok(await business.GetFilms());
    }
}