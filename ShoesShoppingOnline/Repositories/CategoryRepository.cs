using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public void AddCategory(Category category) => CategoryDAO.SaveCategory(category);

        public void DeleteCategory(Category category) => CategoryDAO.DeleteCategory(category);

        public List<Category> GetAllCategory() => CategoryDAO.GetAllCategorys();

        public Category GetCategoryById(int id) => CategoryDAO.GetCategoryById(id);

        public void UpdateCategory(Category category) => CategoryDAO.UpdateCategory(category);
    }
}
