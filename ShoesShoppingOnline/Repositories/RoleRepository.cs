using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public void AddRole(Role role) => RoleDAO.SaveRole(role);

        public void DeleteRole(Role role) => RoleDAO.DeleteRole(role);

        public List<Role> GetAllRoles() => RoleDAO.GetAllRoles();

        public Role GetRoleById(int id) => RoleDAO.GetRoleById(id);

        public void UpdateRole(Role role) => RoleDAO.UpdateRole(role);
    }
}
