using System.Threading.Tasks;
using API.Interop;
using MeetMusicModels.InMemoryModels;
using MeetMusicModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TastesController : Controller
    {
        private readonly ITastesManagementService _tastesManagementService;
        
        public TastesController(ITastesManagementService tastesManagementService)
        {
            _tastesManagementService = tastesManagementService;
        }

        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(MusicGenre[]), 200)]
        [HttpGet]
        [Route("genres")]
        public async Task<IActionResult> GetAllGenres()
        {
            return Ok(await _tastesManagementService.GetAllGenres());
        }

        /// <summary>
        /// Get all families
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(MusicFamily[]), 200)]
        [HttpGet]
        [Route("families")]
        public async Task<IActionResult> GetAllFamilies()
        {
            return Ok(await _tastesManagementService.GetAllFamilies());
        }

        /// <summary>
        /// Updates music families, genres, and genre/family association, based on the given JSON model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(MusicFamilyUpdateModel[]), 200)]
        [HttpPut]
        [Route("families")]
        public async Task<IActionResult> UpdateFamilies([FromBody] MusicFamilyUpdateModel[] model)
        {
            await _tastesManagementService.UpdateFamilies(model);
            return Ok();
        }
    }
}