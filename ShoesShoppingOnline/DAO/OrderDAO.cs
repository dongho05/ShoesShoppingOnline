using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class OrderDAO
    {
        public static List<Order> GetAllOrders()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.Orders.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Order GetOrderById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var order = connection.Orders.Where(x => x.OrderId == id).FirstOrDefault();
                    return order;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveOrder(Order order)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateOrder(Order order)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<Order>(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteOrder(Order order)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.Orders.SingleOrDefault(c => c.OrderId == order.OrderId);
                    context.Orders.Remove(p1);
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
