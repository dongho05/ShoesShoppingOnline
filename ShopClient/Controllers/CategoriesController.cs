using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Categories;
using ShopClient.Models;
using System.Net.Http.Headers;

namespace ShopClient.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;

        public CategoriesController(IConfiguration configuration, INotyfService toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            _toastNotification = toastNotification;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
        public async Task<ActionResult> Index(int currentPage)
        {
            var session = HttpContext.Session.GetString("currentuser");
            if (session != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(session);
                ViewData["Name"] = currentUser.FullName;
                ViewData["Role"] = currentUser.RoleId;

            }

            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest($"api/Categories", RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var categories = JsonConvert.DeserializeObject<List<Category>>(response.Content);

            ViewData["NumberOfPages"] = categories.Count / 6;

            categories = categories.Skip(6 * currentPage).Take(6).ToList();


            ViewData["currentPage"] = currentPage;


            return View(categories);
        }

        // GET: ProductsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest($"api/Categories/" + id, RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var categorie = JsonConvert.DeserializeObject<Category>(response.Content);

            return View(categorie);
        }

        // GET: ProductsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("CategoryId,CategoryName")] CategoryRequest request)
        {


            var token = HttpContext.Session.GetString("AuthToken");

            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest("api/Categories", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                //requesrUrl.AddHeader("authorization", "Bearer " + token);
                var body = new Category
                {
                    CategoryName = request.CategoryName
                };
                var tokenAuth = "Bearer " + token;
                //requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    _toastNotification.Success("Thêm sản phẩm thành công !");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index", "Categories");
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> Edit(int categoryId)
        {
            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest("api/Categories/" + categoryId, RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var category = JsonConvert.DeserializeObject<Category>(response.Content);


            return View(category);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int categoryId, [Bind("CategoryId,CategoryName")] CategoryRequest request)
        {


            var token = HttpContext.Session.GetString("AuthToken");
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Categories/{categoryId}", RestSharp.Method.Put);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new Category
                {
                    CategoryName = request.CategoryName
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _toastNotification.Success("Cập nhật sản phẩm thành công !");
                }


            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index", "Categories");
        }



        // POST: ProductsController/Delete/5
        public async Task<ActionResult> Delete(int categoryId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Categories/{categoryId}", RestSharp.Method.Delete);
                requesrUrl.AddHeader("content-type", "application/json");

                var response = await client.ExecuteAsync(requesrUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _toastNotification.Success("Xóa sản phẩm thành công !");
                }

            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index", "Categories");
        }
    }
}
