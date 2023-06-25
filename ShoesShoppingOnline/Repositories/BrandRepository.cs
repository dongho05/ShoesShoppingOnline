using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        public void AddBrand(Brand brand) => BrandDAO.SaveBrand(brand);

        public void DeleteBrand(Brand brand) => BrandDAO.DeleteBrand(brand);

        public List<Brand> GetAllBrands() => BrandDAO.GetAllBrands();

        public Brand GetBrandById(int id) => BrandDAO.GetBrandById(id);

        public void UpdateBrand(Brand brand) => BrandDAO.UpdateBrand(brand);
    }
}
