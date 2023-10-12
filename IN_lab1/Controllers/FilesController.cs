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

        public IActionResult GetFile(Guid id)
        {
            UploadedFile? file = _uploadedFileService.GetUploadedFile(id);
            if(file is null)
            {
                throw new InvalidOperationException("File with id " + id + "not exists!");
            }

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles", file.Id.ToString());
            string fileType = "application/octet-stream";

            return PhysicalFile(filePath, fileType, file.OriginalFileName!);
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
