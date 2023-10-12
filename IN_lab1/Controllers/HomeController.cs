using IN_lab1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IN_lab1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult GetFile()
        {
            string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/hello.txt");
            string file_type = "text/plain";
            string file_name = "hello.txt";
            return PhysicalFile(file_path, file_type, file_name);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}