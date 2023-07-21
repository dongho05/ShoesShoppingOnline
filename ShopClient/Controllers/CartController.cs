using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Cart;
using ShopClient.Models;

namespace ShopClient.Controllers
{
    public class CartController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly INotyfService _toastNotification;

        private string ApiPort = "";

        public CartController(IConfiguration configuration, INotyfService toastNotification)
        {
            this.configuration = configuration;
            _toastNotification = toastNotification;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
        // GET: CartController
        public ActionResult Index()
        {
            var session = HttpContext.Session.GetString("currentuser");
            if (session != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(session);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;
                var user = GetUserByUserName(currentUser.UserName);
                ViewData["UserId"] = user.Result.UserId;
            }

            var carts = GetCarts().Result.ToList();


            if (carts != null)
            {
                List<Product> products = new List<Product>();
                foreach (var item in carts)
                {
                    products.Add(GetProductById(item.ProductId).Result);
                }
                var joinedData = carts.Select(c => new { Cart = c, Product = products.FirstOrDefault(p => p.ProductId == c.ProductId) })
                         .ToList();


                ViewData["products"] = joinedData;
                return View(carts);
            }
            string notice = "Giỏ hàng đang trống. Hãy mua sản phẩm. ";
            ViewData["notice"] = notice;
            return View();
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users/" + userId, RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var user = JsonConvert.DeserializeObject<User>(response.Content);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
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

        //public async Task<ActionResult> SaveCartToDB(Cart cart)
        //{
        //    try
        //    {
        //        RestClient client = new RestClient(ApiPort);
        //        var requesrUrl = new RestRequest($"api/Carts", RestSharp.Method.Post);
        //        requesrUrl.AddHeader("content-type", "application/json");
        //        requesrUrl.AddParameter("application/json-patch+json", cart, ParameterType.RequestBody);
        //        var response = await client.ExecuteAsync(requesrUrl);
        //        return NoContent();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        public ActionResult AddToCart(int productId)
        {

            var session = HttpContext.Session.GetString("currentuser");
            int userId = 0;
            if (session != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(session);
                var getUser = GetUserByUserName(currentUser.UserName).Result;
                ViewData["Name"] = currentUser.FullName;
                userId = getUser.UserId;

            }
            else
            {
                userId = 999999;
            }

            var product = GetProductById(productId).Result;

            var carts = GetCarts().Result.ToList();
            var findItem = carts.Find(x => x.ProductId == productId);
            if (findItem != null)
            {
                findItem.Quantity++;
            }
            else
            {
                carts.Add(new CartRequest
                {
                    ProductId = productId,
                    Quantity = 1,
                    UserId = userId,
                    UnitPrice = product.UnitPrice - ((product.UnitPrice * product.Discount ?? 0.0) / 100)
                });
            }
            var sessionCarts = JsonConvert.SerializeObject(carts);
            HttpContext.Session.SetString("Carts", sessionCarts);

            //var user = GetUserById(userId).Result;
            //if (user != null)
            //{
            //    foreach (var item in carts)
            //    {
            //        var cartItemDB = new Cart
            //        {
            //            ProductId = item.ProductId,
            //            UserId = user.UserId,
            //            Quantity = item.Quantity,
            //            UnitPrice = item.UnitPrice
            //        };
            //        SaveCartToDB(cartItemDB);
            //    }
            //}


            _toastNotification.Success("Thêm sản phẩm thành công !");

            //return lại chính trang đã nhấn add to cart
            string refererUrl = Request.Headers["Referer"].ToString();
            return Redirect(refererUrl);
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

        //public async Task<List<Cart>> GetCartsByUserId(int userId)
        //{
        //    try
        //    {
        //        RestClient client = new RestClient(ApiPort);
        //        var requesrUrl = new RestRequest($"api/Carts/cart-by-userId/{userId}", RestSharp.Method.Get);
        //        requesrUrl.AddHeader("content-type", "application/json");
        //        var response = await client.ExecuteAsync(requesrUrl);
        //        var carts = JsonConvert.DeserializeObject<List<Cart>>(response.Content);
        //        return carts;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


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

        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove("Carts");
        }
        [HttpGet]
        public IActionResult UpdateCart(int productid, int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var carts = GetCarts().Result.ToList();
            var cartitem = carts.Find(p => p.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.Quantity = quantity;
            }
            var sessionCarts = JsonConvert.SerializeObject(carts);
            HttpContext.Session.SetString("Carts", sessionCarts);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        public IActionResult RemoveCart(int productid)
        {
            var carts = GetCarts().Result.ToList();
            var cartitem = carts.Find(p => p.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                carts.Remove(cartitem);
            }
            var sessionCarts = JsonConvert.SerializeObject(carts);
            HttpContext.Session.SetString("Carts", sessionCarts);
            return RedirectToAction(nameof(Index));
        }

    }
}
