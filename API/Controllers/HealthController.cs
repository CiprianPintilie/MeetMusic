using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
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
        
        [HttpGet]
        [Route("secret")]
        public IActionResult Status(string code)
        {
            return Ok(User.Identity.Name);
        }
    }
}