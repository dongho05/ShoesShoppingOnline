using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Cart;
using ShopClient.DTO.Request.Checkout;
using ShopClient.DTO.Request.Orders;
using ShopClient.Models;

namespace ShopClient.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly INotyfService _toastNotification;

        private string ApiPort = "";

        public CheckoutController(IConfiguration configuration, INotyfService toastNotification)
        {
            this.configuration = configuration;
            _toastNotification = toastNotification;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
        public IActionResult Index()
        {
            var sessionUser = HttpContext.Session.GetString("currentuser");
            if (sessionUser != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(sessionUser);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;
                var user = GetUserByUserName(currentUser.UserName);
                ViewData["UserId"] = user.Result.UserId;
            }

            var carts = GetCarts().Result.ToList();

            if (carts != null)
            {
                List<Product> products = new List<Product>();
                double totalPrice = 0.0f;
                foreach (var item in carts)
                {
                    products.Add(GetProductById(item.ProductId).Result);
                }
                var joinedData = carts.Select(c => new { Cart = c, Product = products.FirstOrDefault(p => p.ProductId == c.ProductId) })
                         .ToList();

                foreach (var item in carts)
                {
                    totalPrice = totalPrice + (item.UnitPrice * item.Quantity);
                }

                ViewData["totalPrice"] = totalPrice;
                ViewData["products"] = joinedData;

                return View();
            }
            return View();
        }
        public CheckoutRequest GetBillInfo()
        {
            var json = HttpContext.Session.GetString("billInfo");
            var billInfo = JsonConvert.DeserializeObject<CheckoutRequest>(json);
            return billInfo;
        }
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            var sessionUser = HttpContext.Session.GetString("currentuser");
            var carts = GetCarts().Result.ToList();
            if (sessionUser != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(sessionUser);
                var user = GetUserByUserName(currentUser.UserName).Result;
                if (user != null)
                {
                    var orderId = SaveCheckoutToOrder(new OrderRequest { UserId = user.UserId, Amount = GetTotalPrice() }).Result;
                    foreach (var item in carts)
                    {
                        var cart = new Cart
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            UserId = user.UserId
                        };
                        SaveCartItemToDB(cart, orderId);
                    }
                }
                //checkout => print bill
                request.TotalPrice = GetTotalPrice();
                var billInfoJson = JsonConvert.SerializeObject(request);
                HttpContext.Session.SetString("billInfo", billInfoJson);

                //ClearCart();
                return View();
            }

            return RedirectToAction("Index", "Login");
        }



        public async Task<ActionResult> PrintBilling()
        {
            var sessionUser = HttpContext.Session.GetString("currentuser");
            if (sessionUser != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(sessionUser);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;
                var user = GetUserByUserName(currentUser.UserName);
                ViewData["UserId"] = user.Result.UserId;
            }

            var carts = GetCarts().Result.ToList();
            if (carts != null)
            {
                List<Product> products = new List<Product>();
                double totalPrice = 0.0f;
                foreach (var item in carts)
                {
                    products.Add(GetProductById(item.ProductId).Result);
                }
                var joinedData = carts.Select(c => new { Cart = c, Product = products.FirstOrDefault(p => p.ProductId == c.ProductId) })
                         .ToList();

                foreach (var item in carts)
                {
                    totalPrice = totalPrice + (item.UnitPrice * item.Quantity);
                }

                ViewData["totalPrice"] = totalPrice;
                ViewData["products"] = joinedData;

            }

            ViewData["billInfo"] = GetBillInfo();
            ClearCart();
            return View();
        }


        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove("Carts");
        }
        public double GetTotalPrice()
        {
            double totalPrice = 0.0f;

            var carts = GetCarts().Result.ToList();

            if (carts != null)
            {
                foreach (var item in carts)
                {
                    totalPrice = totalPrice + (item.UnitPrice * item.Quantity);
                }
                return totalPrice;
            }
            return 0;
        }
        public async Task<List<CartRequest>> GetCarts()
        {

            var session = HttpContext.Session.GetString("Carts");
            if (session != null)
            {
                var carts = JsonConvert.DeserializeObject<List<CartRequest>>(session);
                return carts;
            }
            return new List<CartRequest>();
        }
        public async Task<User> GetUserByUserName(string userName)
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users/get-user-by-username/{userName}", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var response = await client.ExecuteAsync(requesrUrl);
                var user = JsonConvert.DeserializeObject<User>(response.Content);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Products/" + productId, RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var products = JsonConvert.DeserializeObject<Product>(response.Content);
                return products;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ActionResult> SaveCartItemToDB(Cart cart, int orderID)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var body = new OrderDetail
                {
                    CreatedDate = DateTime.Now,
                    ProductId = cart.ProductId,
                    OrderId = orderID,
                    PricePerOne = cart.UnitPrice,
                    Quantity = cart.Quantity,

                };
                var requesrUrl = new RestRequest($"api/OrderDetails", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> SaveCheckoutToOrder(OrderRequest request)
        {
            RestClient client = new RestClient(ApiPort);
            var body = new Order
            {
                Amount = request.Amount,
                OrderDate = DateTime.Now,
                UserId = request.UserId,
            };
            var requesrUrl = new RestRequest($"api/Orders", RestSharp.Method.Post);
            requesrUrl.AddHeader("content-type", "application/json");
            requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(requesrUrl);
            var result = JsonConvert.DeserializeObject<Order>(response.Content);
            return result.OrderId;
        }

    }
}
