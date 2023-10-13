using IN_lab1.Data;
using IN_lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace IN_lab1.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(string username, string password)
        {
            Role? role = _dbContext.Roles!.Where(i => i.Name!.Equals("user")).FirstOrDefault();
            try
            {
                _dbContext.Users?.Add(new User(username, password, role!));
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

        public User? GetUser(string username)
        {
            return _dbContext.Users?.Include(i => i.Role).FirstOrDefault(u => u.Username!.Equals(username));
        }

        public bool IsUserNameUsed(string username)
        {
            return _dbContext.Users?.Where(u => u.Username!.Equals(username)).Count() > 0;
        }
    }
}
