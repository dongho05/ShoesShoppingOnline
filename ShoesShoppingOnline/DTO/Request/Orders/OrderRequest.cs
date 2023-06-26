namespace ShoesShoppingOnline.DTO.Request.Orders
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double? Amount { get; set; }
    }
}
