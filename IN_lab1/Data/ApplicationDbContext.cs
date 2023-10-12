using IN_lab1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime;

namespace IN_lab1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }

        public DbSet<User>? Users { get; set; }
        public DbSet<UploadedFile>? UploadedFiles { get; set; }
        public DbSet<Role?> Roles { get; set; }

    }
}
