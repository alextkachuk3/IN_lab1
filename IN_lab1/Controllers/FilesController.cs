using IN_lab1.Models;
using IN_lab1.Services.UploadedFilesService;
using IN_lab1.Services.UserService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static System.Collections.Specialized.BitVector32;

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

            if (user is null)
            {
                HttpContext.SignOutAsync();
                return LocalRedirect("~/");
            }

            List<UploadedFile>? files = _uploadedFileService.GetUserFiles(user);

            TempData["Role"] = "user";

            return View(files);
        }

        public IActionResult GetFile(Guid id)
        {
            UploadedFile? file = _uploadedFileService.GetUploadedFile(id);
            if (file is null)
            {
                throw new InvalidOperationException("File with id " + id + "not exists!");
            }

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles", file.Id.ToString());
            string fileType = "application/octet-stream";

            return PhysicalFile(filePath, fileType, file.OriginalFileName!);
        }

        [Authorize]
        [HttpPost]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> IndexAsync()
        {
            User? user = _userService.GetUser(User.Identity!.Name!);

            if (user is null)
            {
                throw new InvalidOperationException("User not exist!");
            }

            var boundary = Request.GetMultipartBoundary();
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();
            Guid id = Guid.NewGuid();

            while (section != null)
            {
                var fileSection = section.AsFileSection();

                if (fileSection != null)
                {
                    var fileName = fileSection.FileName;
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles", id.ToString());

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileSection.FileStream!.CopyToAsync(fileStream);
                    }

                    UploadedFile uploadedFile = new UploadedFile(id, fileName, new FileInfo(filePath).Length, user);

                    _uploadedFileService.UploadFile(uploadedFile, user);

                    return LocalRedirect(Url.Action("Index", "Files")!);
                }

                section = await reader.ReadNextSectionAsync();
            }

            return BadRequest("No file found in the request.");
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteFiles(List<string>? filesIds, bool admin = false)
        {
            User? user = _userService.GetUser(User.Identity!.Name!);

            if (user == null)
            {
                throw new InvalidOperationException("User not exist!");
            }

            if (filesIds == null)
                return LocalRedirect(Url.Action("Index", "Files")!);
            foreach (var id in filesIds)
            {
                _uploadedFileService.DeleteFile(Guid.Parse(id), user);
            }
            if (admin)
            {
                return LocalRedirect(Url.Action("Index", "Admin")!);
            }
            else
            {
                return LocalRedirect(Url.Action("Index", "Files")!);
            }
        }
    }
}
