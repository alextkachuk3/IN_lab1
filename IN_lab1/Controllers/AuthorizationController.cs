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

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User? user = _userService.GetUser(username);

            var check_credentials = CheckCredentials(username, password);

            if(check_credentials is not null)
            {
                TempData["AuthError"] = check_credentials;
                return RedirectToAction("Index", "Home");
            }

            if (user == null)
            {
                TempData["AuthError"] = "User with this username not exists!";
                return RedirectToAction("Index", "Home");
            }
            else if(user.CheckCredentials(password, user.Salt!))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, user!.Role!.Name!)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["AuthError"] = "Wrong password!";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string username, string password)
        {
            var check_credentials = CheckCredentials(username, password);

            if (check_credentials is not null)
            {
                TempData["AuthError"] = check_credentials;
                return RedirectToAction("Index", "Home");
            }

            if (_userService.IsUserNameUsed(username))
            {
                TempData["AuthError"] = "Username already used!";
                return View();
            }
            else 
            {
                _userService.AddUser(username, password);
                return Login(username, password);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return LocalRedirect("~/");
        }

        private string? CheckCredentials(string username, string password)
        {
            if (username is null)
            {
                return "Username is empty";
            }

            if (password is null)
            {
                return "Password is empty";
            }

            if (username.Length > 30)
            {
                return "Length of username is bigger than max!";
            }

            if (username.Length < 5)
            {
                return "Minimal username lenght is 5!";
            }

            if (!Models.User.IsAlphanumeric(username))
            {
                return "Username contains special chars!";
            }

            if (!Models.User.IsAlphanumeric(password))
            {
                return "Username contains special chars!";
            }

            return null;
        }
    }
}
