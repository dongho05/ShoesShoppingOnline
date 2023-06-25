using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(Product product) => ProductDAO.SaveProduct(product);

        public void DeleteProduct(Product product) => ProductDAO.DeleteProduct(product);

        public List<Product> GetAllProducts() => ProductDAO.GetAllProducts();

        public Product GetProductById(int id) => ProductDAO.GetProductById(id);

        public void UpdateProduct(Product product) => ProductDAO.UpdateProduct(product);
    }
}
