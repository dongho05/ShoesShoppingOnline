namespace ShopClient.DTO.Request.Cart
{
    public class CartRequest
    {
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }

    }
}
