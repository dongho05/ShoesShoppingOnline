namespace ShopClient.DTO.Request.Products
{
    public class ProductRequest
    {
        public string ProductName { get; set; } = null!;
        public string? ImageProduct { get; set; }
        public string? Describe { get; set; }
        public int? CategoryId { get; set; }
        public int BrandId { get; set; }
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }
        public double? Discount { get; set; }
        public bool? IsSaled { get; set; }
        public bool? IsActivated { get; set; }
    }
}
