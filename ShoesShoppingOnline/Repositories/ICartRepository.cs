using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Repositories
{
    public interface ICartRepository
    {
        List<Cart> GetAllCarts();
        Cart GetCartById(int id);
        Cart GetCartByUserId(int id);
        void AddCart(Cart cart);
        void UpdateCart(Cart cart);
        void DeleteCart(Cart cart);
    }
}
