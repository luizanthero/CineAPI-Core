using Microsoft.AspNetCore.Mvc;

namespace CineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
            => Ok(new { teste = "teste" });
    }
}