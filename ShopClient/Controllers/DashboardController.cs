using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopClient.Models;

namespace ShopClient.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IConfiguration configuration;
        private string ApiPort = "";

        public DashboardController(ILogger<DashboardController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("currentuser");
            if (session != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(session);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;
            }
            return View();
        }
        public IActionResult Table()
        {
            var session = HttpContext.Session.GetString("currentuser");
            if (session != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(session);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;
            }
            return View();
        }
        public IActionResult Billing()
        {
            var session = HttpContext.Session.GetString("currentuser");
            if (session != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(session);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;
            }
            return View();
        }
    }
}
