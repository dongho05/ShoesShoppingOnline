using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface IOrderDetailRepository
    {
        List<OrderDetail> GetAllOderDetails();
        OrderDetail GetOrderDetailById(int id);
        void AddOrderDetail(OrderDetail orderDetail);
        void UpdateOrderDetail(OrderDetail orderDetail);
        void DeleteOrderDetail(OrderDetail orderDetail);
    }
}
