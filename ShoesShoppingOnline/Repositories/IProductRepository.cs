using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
