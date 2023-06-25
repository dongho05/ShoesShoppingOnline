using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface IRoleRepository
    {
        List<Role> GetAllRoles();
        Role GetRoleById(int id);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
    }
}
