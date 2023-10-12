using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IN_lab1.Models
{
    [PrimaryKey(nameof(Name))]
    public class Role
    {
        public Role() { }

        public Role(string name) => Name = name;

        public string? Name { get; set; }

    }
}
