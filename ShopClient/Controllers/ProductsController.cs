using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Products;
using ShopClient.DTO.Request.Users;
using ShopClient.Models;
using System.Net.Http.Headers;

namespace ShopClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;

        public ProductsController(IConfiguration configuration, INotyfService toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            _toastNotification = toastNotification;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        public async Task<List<Brand>> GetBrands()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Brands", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var brands = JsonConvert.DeserializeObject<List<Brand>>(response.Content);
                return brands;
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
                var categories = JsonConvert.DeserializeObject<List<Category>>(response.Content);
                return categories;
            }
            catch (Exception)
            {

                throw;
            }
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

        // GET: ProductsController
        //public async Task<ActionResult> Index(int currentPage, int ActivationStatus)
        //{
        //    var session = HttpContext.Session.GetString("currentuser");
        //    if (session != null)
        //    {
        //        var currentUser = JsonConvert.DeserializeObject<User>(session);
        //        ViewData["Name"] = currentUser.FullName;
        //        ViewData["Role"] = currentUser.RoleId;

        //    }

        //    RestClient client = new RestClient(ApiPort);
        //    var requesrUrl = new RestRequest($"api/Products", RestSharp.Method.Get);
        //    requesrUrl.AddHeader("content-type", "application/json");
        //    var response = await client.ExecuteAsync(requesrUrl);
        //    var products = JsonConvert.DeserializeObject<List<Product>>(response.Content).Where(x => x.IsActivated == true).ToList();

        //    ViewData["NumberOfPages"] = products.Count / 6;

        //    products = products.Skip(6 * currentPage).Take(6).ToList();

        //    var listBrand = GetBrands();
        //    var listCategory = GetCategories();
        //    ViewData["listBrand"] = listBrand.Result.ToList();
        //    ViewData["listCategory"] = listCategory.Result.ToList();
        //    ViewData["currentPage"] = currentPage;


        //    return View(products);
        //}

        public async Task<ActionResult> Index(int currentPage, int ActivationStatus)
        {
            if (ActivationStatus == 0)
            {
                var session = HttpContext.Session.GetString("currentuser");
                if (session != null)
                {
                    var currentUser = JsonConvert.DeserializeObject<User>(session);
                    ViewData["Name"] = currentUser.FullName;
                    ViewData["Role"] = currentUser.RoleId;

                }

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Products", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var products = JsonConvert.DeserializeObject<List<Product>>(response.Content).Where(x => x.IsActivated == true).ToList();

                ViewData["NumberOfPages"] = products.Count / 6;

                products = products.Skip(6 * currentPage).Take(6).ToList();

                var listBrand = GetBrands();
                var listCategory = GetCategories();
                ViewData["listBrand"] = listBrand.Result.ToList();
                ViewData["listCategory"] = listCategory.Result.ToList();
                ViewData["currentPage"] = currentPage;


                return View("Index", products);
            }
            else
            {
                var session = HttpContext.Session.GetString("currentuser");
                if (session != null)
                {
                    var currentUser = JsonConvert.DeserializeObject<User>(session);
                    ViewData["Name"] = currentUser.FullName;
                    ViewData["Role"] = currentUser.RoleId;

                }

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Products", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var products = JsonConvert.DeserializeObject<List<Product>>(response.Content).ToList();

                ViewData["NumberOfPages"] = products.Count / 6;

                products = products.Skip(6 * currentPage).Take(6).ToList();

                var listBrand = GetBrands();
                var listCategory = GetCategories();
                ViewData["listBrand"] = listBrand.Result.ToList();
                ViewData["listCategory"] = listCategory.Result.ToList();
                ViewData["currentPage"] = currentPage;


                return View("Index", products);
            }
        }

        public async Task<ActionResult> Filter(int currentPage)
        {
            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest($"api/Products", RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var products = JsonConvert.DeserializeObject<List<Product>>(response.Content);

            ViewData["NumberOfPages"] = products.Count / 6;

            products = products.Skip(6 * currentPage).Take(6).ToList();

            int brandId = 0, categoryId = 0;

            if (!String.IsNullOrEmpty(Request.Form["BrandId"]))
            {

                brandId = int.Parse(Request.Form["BrandId"]);
            }
            if (!String.IsNullOrEmpty(Request.Form["CategoryId"]))
            {
                categoryId = int.Parse(Request.Form["CategoryId"]);
            }


            if (brandId != 0)
            {
                products = products.Where(x => x.BrandId == brandId).ToList();
                if (categoryId != 0)
                {
                    products = products.Where(x => x.CategoryId == categoryId).ToList();
                }
            }
            else
            {
                if (categoryId != 0)
                {
                    products = products.Where(x => x.CategoryId == categoryId).ToList();
                }
            }

            float minPrice = 0.0f, maxPrice = 0.0f;

            if (!String.IsNullOrEmpty(Request.Form["MinPrice"]))
            {
                minPrice = float.Parse(Request.Form["MinPrice"]);
            }
            if (!String.IsNullOrEmpty(Request.Form["MaxPrice"]))
            {
                maxPrice = float.Parse(Request.Form["MaxPrice"]);
            }
            if (minPrice > 0.0f)
            {
                products = products.Where(x => x.UnitPrice >= minPrice).ToList();
                if (maxPrice > 0 && maxPrice > minPrice)
                {
                    products = products.Where(x => x.UnitPrice <= maxPrice && x.UnitPrice >= minPrice).ToList();
                }
            }
            else
            {
                if (maxPrice > 0.0f)
                {
                    products = products.Where(x => x.UnitPrice <= maxPrice).ToList();
                }
            }

            var listBrand = GetBrands();
            var listCategory = GetCategories();
            ViewData["listBrand"] = listBrand.Result.ToList();
            ViewData["listCategory"] = listCategory.Result.ToList();
            ViewData["currentPage"] = currentPage;
            ViewData["brandId"] = brandId;
            ViewData["categoryId"] = categoryId;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;


            return View("Index", products);
        }

        // GET: ProductsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest($"api/Products/" + id, RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var product = JsonConvert.DeserializeObject<Product>(response.Content);


            var listBrand = GetBrands().Result.ToList().Where(x => x.BrandId == product.BrandId);
            var listCategory = GetCategories().Result.ToList().Where(x => x.CategoryId == product.CategoryId);

            var relatedProduct = GetProducts().Result.ToList().Where(x => x.CategoryId == product.CategoryId).Where(x => x.IsActivated == true).Take(4);


            ViewData["relatedProduct"] = relatedProduct.ToList();
            ViewData["brandName"] = listBrand.FirstOrDefault().BrandName;
            ViewData["categoryName"] = listCategory.FirstOrDefault().CategoryName;

            return View(product);
        }

        // GET: ProductsController/Create
        public IActionResult Create()
        {
            var listBrand = GetBrands();
            var listCategory = GetCategories();
            ViewData["BrandId"] = new SelectList(listBrand.Result.ToList(), "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(listCategory.Result.ToList(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(List<IFormFile> files, [Bind("ProductId,ProductName,ImageProduct,Describe,CategoryId,BrandId,UnitPrice,UnitInStock,Discount,IsSaled,IsActivated")] ProductRequest request)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = formFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    filePaths.Add("/images/" + fileName); // Store the relative path to the file

                }
            }
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");

                try
                {
                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest("api/Products", RestSharp.Method.Post);
                    requesrUrl.AddHeader("content-type", "application/json");
                    //requesrUrl.AddHeader("authorization", "Bearer " + token);
                    var body = new Product
                    {
                        ProductName = request.ProductName,
                        Describe = request.Describe,
                        BrandId = request.BrandId,
                        CategoryId = request.CategoryId,
                        Discount = request.Discount,
                        ImageProduct = filePaths[0],
                        IsActivated = request.IsActivated,
                        IsSaled = request.IsSaled,
                        UnitInStock = request.UnitInStock,
                        UnitPrice = request.UnitPrice,
                    };
                    var tokenAuth = "Bearer " + token;
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
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
            }

            return RedirectToAction("Index", "Products");
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
        // GET: ProductsController/Edit/5
        public async Task<ActionResult> Edit(int productId)
        {
            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest("api/Products/" + productId, RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var product = JsonConvert.DeserializeObject<Product>(response.Content);

            var listBrand = GetBrands();
            var listCategory = GetCategories();
            ViewData["BrandId"] = new SelectList(listBrand.Result.ToList(), "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(listCategory.Result.ToList(), "CategoryId", "CategoryName");

            return View(product);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int productId, List<IFormFile> files, [Bind("ProductId,ProductName,ImageProduct,Describe,CategoryId,BrandId,UnitPrice,UnitInStock,Discount,IsSaled,IsActivated")] ProductRequest request)
        {
            var filePaths = new List<string>();
            if (files.Count > 0)
            {
                long size = files.Sum(f => f.Length);

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var fileName = formFile.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        filePaths.Add("/images/" + fileName); // Store the relative path to the file

                    }
                }
            }
            else
            {
                filePaths.Add(request.ImageProduct);
            }
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;
                try
                {
                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Products/{productId}", RestSharp.Method.Put);
                    requesrUrl.AddHeader("content-type", "application/json");
                    var body = new Product
                    {
                        ProductName = request.ProductName,
                        Describe = request.Describe,
                        BrandId = request.BrandId,
                        CategoryId = request.CategoryId,
                        Discount = request.Discount,
                        ImageProduct = filePaths[0],
                        IsActivated = request.IsActivated,
                        IsSaled = request.IsSaled,
                        UnitInStock = request.UnitInStock,
                        UnitPrice = request.UnitPrice,
                    };
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                    requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                    var response = await client.ExecuteAsync(requesrUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        _toastNotification.Success("Cập nhật sản phẩm thành công !");
                        return RedirectToAction("Index", "Products");

                    }


                }
                catch (Exception)
                {

                    throw;
                }
            }


            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }



        // POST: ProductsController/Delete/5
        public async Task<ActionResult> Delete(int productId)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;
                try
                {
                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Products/{productId}", RestSharp.Method.Delete);
                    requesrUrl.AddHeader("content-type", "application/json");
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                    var response = await client.ExecuteAsync(requesrUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        _toastNotification.Success("Xóa sản phẩm thành công !");
                        return RedirectToAction("Index", "Products");

                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }

            _toastNotification.Error("Bạn không có quyền xóa sản phẩm !");
            return View("Index");
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> FileUpload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = formFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    filePaths.Add("/images/" + fileName); // Store the relative path to the file

                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, filePaths });
        }

        public async Task<ActionResult> SearchProduct()
        {
            var request = Request.Form["name_product_searching"];
            var listProducs = GetProducts().Result.ToList();
            listProducs = listProducs.Where(x => x.ProductName.ToLower().Contains(request.ToString().ToLower())).ToList();

            var listBrand = GetBrands();
            var listCategory = GetCategories();
            ViewData["listBrand"] = listBrand.Result.ToList();
            ViewData["listCategory"] = listCategory.Result.ToList();

            return View("Index", listProducs);
        }

    }
}
