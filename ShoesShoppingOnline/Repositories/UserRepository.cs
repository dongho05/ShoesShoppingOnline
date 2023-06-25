using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void AddUser(User user) => UserDAO.SaveUser(user);

        public void DeleteUser(User user) => UserDAO.DeleteUser(user);

        public List<User> GetAllUsers() => UserDAO.GetAllUsers();

        public User GetUserById(int id) => UserDAO.GetUserById(id);

        public void UpdateUser(User user) => UserDAO.UpdateUser(user);
    }
}
