using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class ProductDAO
    {
        public static List<Product> GetAllProducts()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.Products.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Product GetProductById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var product = connection.Products.Where(x => x.ProductId == id).FirstOrDefault();
                    return product;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveProduct(Product product)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Products.Add(product);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateProduct(Product product)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<Product>(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteProduct(Product product)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.Products.SingleOrDefault(c => c.ProductId == product.ProductId);
                    context.Products.Remove(p1);
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
