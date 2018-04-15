using System.Threading.Tasks;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetMusic.Controllers
{
    public class UserController : Controller
    {

        private readonly MeetMusicDbContext _context;

        public UserController(MeetMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToArrayAsync();
            return View(users);
        }

        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            user.Deleted = !user.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }
    }
}