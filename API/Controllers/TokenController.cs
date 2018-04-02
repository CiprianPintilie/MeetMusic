using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        public IActionResult Callback(string code)
        {
            return Ok();
        }
    }
}