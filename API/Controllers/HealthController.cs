using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public IActionResult Status()
        {
            return Ok("API started");
        }
    }
}