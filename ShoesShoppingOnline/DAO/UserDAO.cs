using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class UserDAO
    {
        public static List<User> GetAllUsers()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.Users.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static User GetUserById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var user = connection.Users.Where(x => x.UserId == id).FirstOrDefault();
                    return user;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveUser(User user)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateUser(User user)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<User>(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteUser(User user)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.Users.SingleOrDefault(c => c.UserId == user.UserId);
                    context.Users.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
