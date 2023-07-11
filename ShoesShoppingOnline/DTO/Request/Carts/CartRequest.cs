namespace ShoesShoppingOnline.DTO.Request.Carts
{
    public class CartRequest
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
