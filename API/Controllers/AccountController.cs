using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task Login(string returnUrl = "/api/health")
        {
            //await HttpContext.ChallengeAsync("Spotify", new AuthenticationProperties { RedirectUri = returnUrl });
        }

        [Authorize]
        [HttpGet]
        [Route("logout")]
        public async Task Logout()
        {
            // Clear the local session cookie
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //// Construct the post-logout URL (i.e. where we'll tell Auth0 to redirect after logging the user out)
            //var request = HttpContext.Request;
            //string postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + Url.Action("Index", "Home");

            // Redirect to the spotify logout endpoint
            //string logoutUri = $"https://{_configuration["Auth0:Domain"]}/v2/logout?client_id={_configuration["Auth0:ClientId"]}&returnTo={Uri.EscapeDataString(postLogoutUri)}";
            //HttpContext.Response.Redirect(logoutUri);
        }
    }
}