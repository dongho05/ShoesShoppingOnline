using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.Models;
using System.Diagnostics;

namespace ShopClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        private string ApiPort = "";

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
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
        public async Task<IActionResult> Index()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Products", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var products = JsonConvert.DeserializeObject<List<Product>>(response.Content);

                var session = HttpContext.Session.GetString("currentuser");
                if (session != null)
                {
                    var currentUser = JsonConvert.DeserializeObject<User>(session);
                    ViewData["Name"] = currentUser.FullName;
                    ViewData["Role"] = currentUser.RoleId;
                    var user = GetUserByUserName(currentUser.UserName);
                    ViewData["UserId"] = user.Result.UserId;
                }

                var saledProduct = GetTopProductIsSaled().Result.ToList();

                var productsBuyMost = GetProductsBuyMost().Result.ToList();

                ViewData["saledProduct"] = saledProduct;
                ViewData["productsBuyMost"] = productsBuyMost;

                return View(products);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Product>> GetTopProductIsSaled()
        {
            var list = GetProducts().Result.ToList();
            list = list.Where(x => x.IsSaled == true).Take(3).ToList();
            return list;
        }

        public async Task<List<Product>> GetProductsBuyMost()
        {
            var orderDs = GetOrderDetails().Result.ToList();
            var products = GetProducts().Result.ToList();

            var productsBuyMost = orderDs.GroupBy(d => d.ProductId).Select(g => new
            {
                ProductId = g.Key,
                TotalQuantity = g.Sum(d => d.Quantity)
            }).OrderByDescending(d => d.TotalQuantity).ToList();

            var result = productsBuyMost.Join(products, p => p.ProductId, prod => prod.ProductId, (p, prod) => new Product
            {
                ProductId = p.ProductId,
                ProductName = prod.ProductName,
            }).ToList();
            return result;
        }
        public async Task<List<Product>> GetProducts()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Products", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var products = JsonConvert.DeserializeObject<List<Product>>(response.Content);
                return products;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<OrderDetail>> GetOrderDetails()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/OrderDetails", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var orderDs = JsonConvert.DeserializeObject<List<OrderDetail>>(response.Content);
                return orderDs;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}