using System.ComponentModel.DataAnnotations;

namespace IN_lab1.Models
{

    public class UploadedFile
    {
        public UploadedFile() { }

        public UploadedFile(Guid id, string originalFileName, User user)
        {
            Id = Id;
            OriginalFileName = originalFileName;
            User = user;
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string? OriginalFileName { get; set; }

        [Required]
        public User? User { get; set; }
    }
}
