using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void AddOrder(Order order) => OrderDAO.SaveOrder(order);

        public void DeleteOrder(Order order) => OrderDAO.DeleteOrder(order);

        public List<Order> GetAllOrders() => OrderDAO.GetAllOrders();

        public Order GetOrderById(int id) => OrderDAO.GetOrderById(id);

        public void UpdateOrder(Order order) => OrderDAO.UpdateOrder(order);
    }
}
