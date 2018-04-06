using System;
using System.Threading.Tasks;
using API.Interop;
using MeetMusicModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly string _apiUrl;

        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _apiUrl = configuration["ApiInfo:ApiUrl"];
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
        /// Get a specific user using his id
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(User), 200)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(User), 201)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data in model");
            var createdUser = await _userService.CreateUser(user);
            return Created($"{_apiUrl}/users/{createdUser.Id}", createdUser);
        }
    }
}