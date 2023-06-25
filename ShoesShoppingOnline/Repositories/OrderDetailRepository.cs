using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.SaveOrderDetail(orderDetail);

        public void DeleteOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.DeleteOrderDetail(orderDetail);

        public List<OrderDetail> GetAllOderDetails() => OrderDetailDAO.GetAllOrderDetails();

        public OrderDetail GetOrderDetailById(int id) => OrderDetailDAO.GetOrderDetailById(id);

        public void UpdateOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.UpdateOrderDetail(orderDetail);
    }
}
