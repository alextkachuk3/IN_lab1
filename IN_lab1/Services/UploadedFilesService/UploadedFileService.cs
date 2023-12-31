﻿using IN_lab1.Data;
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

        public void AdminDeleteFile(Guid id, User user)
        {
            if (!user.Role!.Name!.Equals("admin")) 
            {
                throw new InvalidOperationException("User trying to delete files as admin!");
            }

            DeleteFile(id, user);
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
                        File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles", id.ToString()));
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
            return _dbContext.UploadedFiles?.Include(i => i.User).ToList();
        }

        public UploadedFile? GetUploadedFile(Guid id)
        {
            return _dbContext.UploadedFiles?.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public List<UploadedFile>? GetUserFiles(User user)
        {
            return _dbContext.UploadedFiles?.Where(i => i.User!.Equals(user)).ToList();
        }

        public void UploadFile(UploadedFile file, User user)
        {
            try
            {
                _dbContext.UploadedFiles?.Add(new UploadedFile(file.Id, file.OriginalFileName!, file.FileSize, user));
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
    }
}
