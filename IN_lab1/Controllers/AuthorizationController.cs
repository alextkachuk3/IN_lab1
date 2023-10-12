using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IN_lab1.Services.UserService;
using IN_lab1.Models;

namespace IN_lab1.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IUserService _userService;

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User? user = _userService.GetUser(username);

            if(user == null)
            {
                ViewBag.AuthError = "User with this username not exists!";
                return View();
            }
            else if(user.CheckCredentials(password))
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, username));

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect("~/");
            }
            else
            {
                ViewBag.AuthError = "Wrong password!";
                return View();
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string username, string password, string firstName, string lastName)
        {
            if(_userService.IsUserNameUsed(username))
            {
                ViewBag.AuthError = "Username already used!";
                return View();
            }
            else 
            {
                _userService.AddUser(username, password, firstName, lastName);
                return Login(username, password);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return LocalRedirect("~/");
        }
    }
}
