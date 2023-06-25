using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.DAO
{
    public class OrderDetailDAO
    {
        public static List<OrderDetail> GetAllOrderDetails()
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var list = connection.OrderDetails.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static OrderDetail GetOrderDetailById(int id)
        {
            try
            {
                using (var connection = new ShoesShoppingOnlineContext())
                {
                    var orderDetail = connection.OrderDetails.Where(x => x.DetailId == id).FirstOrDefault();
                    return orderDetail;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.OrderDetails.Add(orderDetail);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    context.Entry<OrderDetail>(orderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static void DeleteOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var context = new ShoesShoppingOnlineContext())
                {
                    var p1 = context.OrderDetails.SingleOrDefault(c => c.DetailId == orderDetail.DetailId);
                    context.OrderDetails.Remove(p1);
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
