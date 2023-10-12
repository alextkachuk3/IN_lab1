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
            string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/test.txt");
            string file_type = "application/octet-stream";
            string file_name = "hello.txt";
            return PhysicalFile(file_path, file_type, file_name);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = await reader.ReadToEndAsync();
                    Console.WriteLine("File Content:");
                    Console.WriteLine(fileContent);
                }

                var uploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                //var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var uniqueFileName = "test.txt";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            else
            {
                Console.WriteLine("Invalid file or empty content.");
            }
            return Index();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}