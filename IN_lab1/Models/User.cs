using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime;

namespace IN_lab1.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public User() { }

        public User(string username, string password, Role role)
        {
            Username = username;
            PasswordHash = password;
            Role = role;
        }

        public bool CheckCredentials(string password)
        {
            return PasswordHash!.Equals(password);
        }

        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 4)]
        public string? Username { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public Role? Role { get; set; }
    }
}
