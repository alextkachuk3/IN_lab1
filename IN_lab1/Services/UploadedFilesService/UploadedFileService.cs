using IN_lab1.Data;
using IN_lab1.Models;
using System;

namespace IN_lab1.Services.UploadedFilesService
{
    public class UploadedFileService : IUploadedFileService
    {
        private readonly ApplicationDbContext _dbContext;

        public UploadedFileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteFile(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<UploadedFile>? GetUserFiles(User user)
        {
            return _dbContext.UploadedFiles?.Where(i => i.User == user).ToList();
        }

        public async Task UploadFileAsync(IFormFile file, User user)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                Guid fileID = Guid.NewGuid();
                var filePath = Path.Combine(uploadsFolder, fileID.ToString());

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                try
                {
                    _dbContext.UploadedFiles?.Add(new UploadedFile(fileID, file.FileName, file.Length, user));
                }
                catch
                {
                    throw;
                }
                finally
                {
                    _dbContext.SaveChanges();
                }

            }
            else
            {
                throw new InvalidDataException("Invalid file or empty content.");
            }
        }
    }
}
