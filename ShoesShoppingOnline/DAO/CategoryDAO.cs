using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class CategoryDAO
    {
        public static List<Category> GetAllCategorys()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.Categories.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Category GetCategoryById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var category = connection.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
                    return category;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveCategory(Category category)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Categories.Add(category);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateCategory(Category category)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<Category>(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteCategory(Category category)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.Categories.SingleOrDefault(c => c.CategoryId == category.CategoryId);
                    context.Categories.Remove(p1);
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
