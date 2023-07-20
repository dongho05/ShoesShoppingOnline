using ShoesShoppingOnline.DAO;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public class CartRepository : ICartRepository
    {
        public void AddCart(Cart cart) => CartDAO.SaveCart(cart);

        public void DeleteCart(Cart cart) => CartDAO.DeleteCart(cart);

        public List<Cart> GetAllCarts() => CartDAO.GetAllCarts();

        public Cart GetCartById(int id) => CartDAO.GetCartById(id);

        public List<Cart> GetCartByUserId(int id) => CartDAO.GetCartByUserId(id);

        public void UpdateCart(Cart cart) => CartDAO.UpdateCart(cart);
    }
}
