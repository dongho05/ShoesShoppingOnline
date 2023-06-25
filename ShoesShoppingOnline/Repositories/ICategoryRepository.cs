using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategory();
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
