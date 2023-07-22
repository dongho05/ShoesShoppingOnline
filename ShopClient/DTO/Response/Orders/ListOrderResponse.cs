namespace ShopClient.DTO.Response.Orders
{
    public class ListOrderResponse
    {
        public int TotalOrdersByDateNow { get; set; }
        public double TotalAmountByMonth { get; set; }
        public int TotalOrdersByMonth { get; set; }

    }
}
