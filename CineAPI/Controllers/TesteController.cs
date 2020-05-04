using Microsoft.AspNetCore.Mvc;

namespace CineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        /// <summary>
        /// teste teste
        /// </summary>
        /// <returns>retorna teste</returns>
        [HttpGet]
        public IActionResult Get()
            => Ok(new { teste = "teste" });
    }
}