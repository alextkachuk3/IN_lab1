using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IN_lab1.Models
{
    public class Role
    {
        public Role() { }

        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

    }
}
