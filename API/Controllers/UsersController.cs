using System;
using System.Threading.Tasks;
using API.Interop;
using MeetMusicModels.InMemoryModels;
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
        [ProducesResponseType(404)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        /// <summary>
        /// Get user's top listened music families
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserMusicFamily[]), 200)]
        [ProducesResponseType(404)]
        [HttpGet]
        [Route("top/families/{id}")]
        public async Task<IActionResult> GetUserTopFamilies(Guid id)
        {
            var musicTastes = await _userService.GetUserTopMusicFamilies(id);
            return Ok(musicTastes);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(409)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data in model");
            var createdUser = await _userService.CreateUser(user);
            return Created($"{_apiUrl}/users/{createdUser.Id}", createdUser);
        }
        
        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] User user, Guid id)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data in model");
            var createdUser = await _userService.UpdateUser(user, id);
            return Ok(createdUser);
        }

        /// <summary>
        /// Updates given user tastes using the given model
        /// </summary>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpPut]
        [Route("tastes/{id}")]
        public async Task<IActionResult> UpdateUserTastes(Guid id, [FromBody] UserMusicFamily[] models)
        {
            await _userService.UpdateUserTastes(id, models);
            return Ok();
        }

        /// <summary>
        /// Activates a deleted user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpPut]
        [Route("activate/{id}")]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            await _userService.ActivateUser(id);
            return Ok();
        }

        /// <summary>
        /// Return matched users
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(MatchModel[]), 200)]
        [ProducesResponseType(404)]
        [HttpPut]
        [Route("{id}/match")]
        public async Task<IActionResult> MatchUser(Guid id, [FromBody] MatchParametersModel model)
        {
            return Ok(await _userService.MatchUser(id, model));
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}