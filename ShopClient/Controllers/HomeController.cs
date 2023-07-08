using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShoesShoppingOnline.Models;
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

        public async Task<IActionResult> Index()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Products", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var products = JsonConvert.DeserializeObject<List<Product>>(response.Content);

                return View(products);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Category>> GetCategories()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Categories", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var list = JsonConvert.DeserializeObject<List<Category>>(response.Content);
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Brand>> GetBrands()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Brands", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var list = JsonConvert.DeserializeObject<List<Brand>>(response.Content);
                return list;
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