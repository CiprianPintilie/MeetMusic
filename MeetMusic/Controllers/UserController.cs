using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewData["Users"] = await _context.Users.ToArrayAsync();
            return View();
        }
    }
}