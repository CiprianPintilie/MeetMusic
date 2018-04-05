using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Interop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public TokenController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Authenticate()
        {
            var email = Request.Form["Email"];
            var password = Request.Form["Password"];

            if (email.Count != 1 || password.Count != 1)
                return BadRequest("Username and password required to authenticate");
            
            var userId = await _userService.AuthenticateUser(email, password);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            var token = new JwtSecurityToken
            (
                _configuration.GetSection("Token")["Issuer"],
                _configuration.GetSection("Token")["Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token")["SignatureKey"])),
                    SecurityAlgorithms.HmacSha256)
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}