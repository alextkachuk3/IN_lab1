using IN_lab1.Models;
using IN_lab1.Services.UploadedFilesService;
using IN_lab1.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IN_lab1.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUploadedFileService _uploadedFileService;

        public AdminController(IUploadedFileService uploadedFileService)
        {
            _uploadedFileService = uploadedFileService;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            List<UploadedFile>? files = _uploadedFileService.GetAllFiles();

            return View("../Files/Index", files);
        }
    }
}
