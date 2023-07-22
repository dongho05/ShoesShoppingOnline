using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Users;
using ShopClient.DTO.Response.Orders;
using ShopClient.Models;

namespace ShopClient.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;

        public DashboardController(ILogger<DashboardController> logger, IConfiguration configuration, INotyfService toastNotification)
        {
            _logger = logger;
            this.configuration = configuration;
            _toastNotification = toastNotification;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
        public async Task<UserRequest> GetCurrentUser()
        {
            var user = HttpContext.Session.GetString("currentuser");
            if (user != null)
            {
                var currentUser = JsonConvert.DeserializeObject<UserRequest>(user);
                return currentUser;
            }
            return null;
        }
        public async Task<List<Order>> GetOrders()
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Orders", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var response = await client.ExecuteAsync(requesrUrl);
                var orders = JsonConvert.DeserializeObject<List<Order>>(response.Content);
                if (orders != null)
                {
                    return orders;

                }
            }

            //_toastNotification.Error("Bạn không có quyền truy cập trang này");
            return null;
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users/" + userId, RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var response = await client.ExecuteAsync(requesrUrl);
                var user = JsonConvert.DeserializeObject<User>(response.Content);
                if (user != null)
                {

                    return user;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("currentuser");
            if (session != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(session);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;

                var orders = GetOrders().Result;
                if (orders != null)
                {
                    int totalOrdersByDateNow = 0;
                    double totalAmountByMonth = 0.0f;
                    totalOrdersByDateNow = orders.Where(x => x.OrderDate == DateTime.Now).ToList().Count;

                    orders = orders.Where(x => x.OrderDate.Month == DateTime.Now.Month).ToList();
                    foreach (var item in orders)
                    {
                        totalAmountByMonth = totalAmountByMonth + item.Amount ?? 0;
                    }
                    var response = new ListOrderResponse
                    {
                        TotalAmountByMonth = totalAmountByMonth,
                        TotalOrdersByDateNow = totalOrdersByDateNow,
                        TotalOrdersByMonth = orders.Count
                    };

                    List<User> users = new List<User>();
                    foreach (var item in orders)
                    {
                        users.Add(GetUserById(item.UserId).Result);
                    }
                    var joinedData = orders.Select(c => new
                    {
                        Order = c,
                        User = users.FirstOrDefault(p => p.UserId == c.UserId)
                    }).OrderByDescending(x => x.Order.OrderDate);

                    ViewData["orderResponse"] = response;
                    ViewData["orders"] = joinedData;
                    return View();
                }
            }

            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

    }
}
