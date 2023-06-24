using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class BrandDAO
    {
        public static List<Brand> GetAllBrands()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.Brands.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Brand GetBrandById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var brand = connection.Brands.Where(x => x.BrandId == id).FirstOrDefault();
                    return brand;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveBrand(Brand brand)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Brands.Add(brand);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateBrand(Brand brand)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<Brand>(brand).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteBrand(Brand brand)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.Brands.SingleOrDefault(c => c.BrandId == brand.BrandId);
                    context.Brands.Remove(p1);
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
