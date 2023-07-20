namespace ShopClient.DTO.Request.Checkout
{
    public class CheckoutRequest
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public double TotalPrice { get; set; }
    }
}
