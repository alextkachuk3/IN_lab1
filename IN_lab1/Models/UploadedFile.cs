using System.ComponentModel.DataAnnotations;

namespace IN_lab1.Models
{

    public class UploadedFile
    {
        public UploadedFile() { }

        public UploadedFile(Guid id, string originalFileName, long fileSize, User user)
        {
            Id = Id;
            OriginalFileName = originalFileName;
            FileSize = fileSize;
            User = user;
            UploadDate = DateTime.Now;
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string? OriginalFileName { get; set; }

        [Required]
        public long FileSize { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public User? User { get; set; }
    }
}
