﻿using IN_lab1.Models;

namespace IN_lab1.Services.UploadedFilesService
{
    public interface IUploadedFileService
    {
        public UploadedFile? GetUploadedFile(Guid id);

        public Task UploadFileAsync(IFormFile file, User user);

        public void DeleteFile(Guid id, string username);

        List<UploadedFile>? GetUserFiles(User user);

        List<UploadedFile>? GetAllFiles();
    }
}
