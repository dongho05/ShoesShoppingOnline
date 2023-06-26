namespace ShoesShoppingOnline.DTO.Request.OrderDetails
{
    public class OrderDetailRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? PricePerOne { get; set; }
        public double? Discount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int OrderId { get; set; }
    }
}
