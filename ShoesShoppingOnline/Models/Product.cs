using System;
using System.Collections.Generic;

namespace ShoesShoppingOnline.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
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

        public virtual Brand Brand { get; set; } = null!;
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
