namespace ShopClient.Models
{
    public partial class OrderDetail
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double PricePerOne { get; set; }
        public double? Discount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
