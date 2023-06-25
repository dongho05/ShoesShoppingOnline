using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface IBrandRepository
    {
        List<Brand> GetAllBrands();
        Brand GetBrandById(int id);
        void AddBrand(Brand brand);
        void UpdateBrand(Brand brand);
        void DeleteBrand(Brand brand);
    }
}
