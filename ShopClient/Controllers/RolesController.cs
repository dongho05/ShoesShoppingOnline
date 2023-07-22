using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Role;
using ShopClient.DTO.Request.Users;
using ShopClient.Models;
using System.Net.Http.Headers;

namespace ShopClient.Controllers
{
    public class RolesController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;

        public RolesController(IConfiguration configuration, INotyfService toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
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
            var requesrUrl = new RestRequest($"api/Roles", RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            var response = await client.ExecuteAsync(requesrUrl);
            var roles = JsonConvert.DeserializeObject<List<Role>>(response.Content);

            if (roles != null)
            {
                ViewData["NumberOfPages"] = roles.Count / 6;

                roles = roles.Skip(6 * currentPage).Take(6).ToList();


                ViewData["currentPage"] = currentPage;
                return View(roles);

            }


            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();

        }

        // GET: ProductsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Roles/" + id, RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var response = await client.ExecuteAsync(requesrUrl);
                var role = JsonConvert.DeserializeObject<Role>(response.Content);
                if (role != null)
                {

                    return View(role);
                }
            }

            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

        // GET: ProductsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("RoleId,RoleName")] RoleRequest request)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;
                try
                {
                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest("api/Roles", RestSharp.Method.Post);
                    requesrUrl.AddHeader("content-type", "application/json");
                    var body = new Role
                    {
                        RoleName = request.RoleName
                    };
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                    requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                    var response = await client.ExecuteAsync(requesrUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        _toastNotification.Success("Thêm quyền thành công !");
                        return RedirectToAction("Index", "Roles");
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

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> Edit(int roleId)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest("api/Roles/" + roleId, RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var response = await client.ExecuteAsync(requesrUrl);
                var role = JsonConvert.DeserializeObject<Role>(response.Content);
                if (role != null)
                {
                    return View(role);

                }
            }


            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int roleId, [Bind("RoleId,RoleName")] RoleRequest request)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;
                try
                {
                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Roles/{roleId}", RestSharp.Method.Put);
                    requesrUrl.AddHeader("content-type", "application/json");
                    var body = new Role
                    {
                        RoleName = request.RoleName
                    };
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                    requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                    var response = await client.ExecuteAsync(requesrUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        _toastNotification.Success("Cập nhật quyền thành công !");
                        return RedirectToAction("Index", "Roles");

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
        public async Task<ActionResult> Delete(int roleId)
        {

            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;
                try
                {
                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Roles/{roleId}", RestSharp.Method.Delete);
                    requesrUrl.AddHeader("content-type", "application/json");
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                    var response = await client.ExecuteAsync(requesrUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        _toastNotification.Success("Xóa quyền thành công !");
                        return RedirectToAction("Index", "Roles");

                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }

            _toastNotification.Error("Bạn không có quyền xóa !");
            return View("Index");
        }
    }
}
