using IN_lab1.Data;
using IN_lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace IN_lab1.Services.UploadedFilesService
{
    public class UploadedFileService : IUploadedFileService
    {
        private readonly ApplicationDbContext _dbContext;

        public UploadedFileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteFile(Guid id, User user)
        {
            UploadedFile? file = _dbContext.UploadedFiles!.Where(i => i.Id.Equals(id)).Include(i => i.User).FirstOrDefault();
            if (file is not null)
            {
                if (file.User!.Equals(user) || user.Role!.Name!.Equals("admin"))
                {
                    try
                    {
                        _dbContext.UploadedFiles!.Remove(file);
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
                    throw new InvalidOperationException("User tried to delete a file that doesn't own!");
                }
            }
        }

        public List<UploadedFile>? GetAllFiles()
        {
            return _dbContext.UploadedFiles?.ToList();
        }

        public UploadedFile? GetUploadedFile(Guid id)
        {
            return _dbContext.UploadedFiles?.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public List<UploadedFile>? GetUserFiles(User user)
        {
            return _dbContext.UploadedFiles?.Where(i => i.User!.Equals(user)).ToList();
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
