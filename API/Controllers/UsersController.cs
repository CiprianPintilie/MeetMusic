using System.Threading.Tasks;
using API.Interop;
using MeetMusicModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all the users
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(User[]), 200)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(User), 200)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data in model");
            var createdUser = await _userService.CreateUser(user);
            return Ok(createdUser);
        }
    }
}