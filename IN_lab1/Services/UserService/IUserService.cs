using IN_lab1.Models;

namespace IN_lab1.Services.UserService
{
    public interface IUserService
    {
        public bool IsUserNameUsed(string username);

        public void AddUser(string username, string password);

        public User? GetUser(string username);
    }
}
