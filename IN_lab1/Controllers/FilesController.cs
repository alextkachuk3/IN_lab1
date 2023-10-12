using IN_lab1.Models;
using IN_lab1.Services.UploadedFilesService;
using IN_lab1.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace IN_lab1.Controllers
{
    public class FilesController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUploadedFileService _uploadedFileService;

        public FilesController(IUserService userService, IUploadedFileService uploadedFileService)
        {
            _userService = userService;
            _uploadedFileService = uploadedFileService;
        }

        public IActionResult Index()
        {
            if (!HttpContext.User!.Identity!.IsAuthenticated)
            {
                return LocalRedirect(Url.Action("Index", "Home")!);
            }

            User? user = _userService.GetUser(User.Identity!.Name!);

            if (user == null)
            {
                throw new InvalidOperationException("User not authorized!");
            }

            List<UploadedFile>? files = _uploadedFileService.GetUserFiles(user);

            return View(files);
        }

        public IActionResult GetFile()
        {
            string file_name = "testfile.txt";
            string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", file_name);
            string file_type = "application/octet-stream";

            return PhysicalFile(file_path, file_type, file_name);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(IFormFile file)
        {
            User? user = _userService.GetUser(User.Identity!.Name!);

            if (user == null)
            {
                throw new InvalidOperationException("User not authorized!");
            }
            await _uploadedFileService.UploadFileAsync(file, user);
            return LocalRedirect(Url.Action("Index", "Files")!);
        }
    }
}
