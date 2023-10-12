using Microsoft.AspNetCore.Mvc;

namespace IN_lab1.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Index()
        {
            if (!HttpContext.User!.Identity!.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("index", "home")!);
            }
            return View();
        }
    }
}
