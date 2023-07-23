using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        User Login(string username, string password);
        User GetUserByUsername(string username);
        void ChangePassword(int userId, string password);
    }
}
