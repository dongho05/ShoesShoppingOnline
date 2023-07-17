using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Brands;
using ShopClient.Models;
using System.Net.Http.Headers;

namespace ShopClient.Controllers
{
    public class BrandsController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;

        public BrandsController(IConfiguration configuration, INotyfService toastNotification)
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
            var requesrUrl = new RestRequest($"api/Brands", RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var brands = JsonConvert.DeserializeObject<List<Brand>>(response.Content);

            ViewData["NumberOfPages"] = brands.Count / 6;

            brands = brands.Skip(6 * currentPage).Take(6).ToList();


            ViewData["currentPage"] = currentPage;


            return View(brands);
        }

        // GET: ProductsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest($"api/Brands/" + id, RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var brand = JsonConvert.DeserializeObject<Brand>(response.Content);

            return View(brand);
        }

        // GET: ProductsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("BrandId,BrandName")] BrandRequest request)
        {


            var token = HttpContext.Session.GetString("AuthToken");

            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest("api/Brands", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                //requesrUrl.AddHeader("authorization", "Bearer " + token);
                var body = new Brand
                {
                    BrandName = request.BrandName
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
            return RedirectToAction("Index", "Brands");
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> Edit(int brandId)
        {
            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest("api/Brands/" + brandId, RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var brand = JsonConvert.DeserializeObject<Brand>(response.Content);


            return View(brand);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int brandId, [Bind("BrandId,BrandName")] BrandRequest request)
        {


            var token = HttpContext.Session.GetString("AuthToken");
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Brands/{brandId}", RestSharp.Method.Put);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new Brand
                {
                    BrandName = request.BrandName
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
            return RedirectToAction("Index", "Brands");
        }



        // POST: ProductsController/Delete/5
        public async Task<ActionResult> Delete(int brandId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Brands/{brandId}", RestSharp.Method.Delete);
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
            return RedirectToAction("Index", "Brands");
        }


    }
}
