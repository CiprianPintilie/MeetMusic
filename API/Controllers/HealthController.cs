using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        /// <summary>
        /// Shows if the api is started
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet]
        [Route("")]
        public IActionResult Status()
        {
            return Ok("API started");
        }
    }
}