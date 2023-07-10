using Microsoft.AspNetCore.Mvc;

namespace ShopClient.Controllers
{
    public class CartController : Controller
    {
        // GET: CartController
        public ActionResult Index(int ProductId)
        {
            var m = Request.Form["Quantity"];

            return View();
        }

    }
}
