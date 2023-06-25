using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
