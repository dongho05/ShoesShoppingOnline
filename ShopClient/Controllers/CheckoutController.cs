using Microsoft.AspNetCore.Mvc;

namespace ShopClient.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
