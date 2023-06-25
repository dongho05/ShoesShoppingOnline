using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class RoleDAO
    {
        public static List<Role> GetAllRoles()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.Roles.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Role GetRoleById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var role = connection.Roles.Where(x => x.RoleId == id).FirstOrDefault();
                    return role;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveRole(Role role)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Roles.Add(role);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateRole(Role role)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<Role>(role).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteRole(Role role)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.Roles.SingleOrDefault(c => c.RoleId == role.RoleId);
                    context.Roles.Remove(p1);
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
