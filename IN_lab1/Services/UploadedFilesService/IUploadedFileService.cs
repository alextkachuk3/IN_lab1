using IN_lab1.Models;

namespace IN_lab1.Services.UploadedFilesService
{
    public interface IUploadedFileService
    {
        public Task UploadFileAsync(IFormFile file, User user);

        public void DeleteFile(Guid id);

        List<UploadedFile>? GetUserFiles(User user);
    }
}
