using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class CartDAO
    {
        public static List<Cart> GetAllCarts()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.Carts.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Cart GetCartById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var category = connection.Carts.Where(x => x.CartId == id).FirstOrDefault();
                    return category;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Cart GetCartByUserId(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var category = connection.Carts.Where(x => x.UserId == id).FirstOrDefault();
                    return category;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveCart(Cart cart)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Carts.Add(cart);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateCart(Cart cart)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<Cart>(cart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteCart(Cart cart)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.Carts.SingleOrDefault(c => c.CartId == cart.CartId);
                    context.Carts.Remove(p1);
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
