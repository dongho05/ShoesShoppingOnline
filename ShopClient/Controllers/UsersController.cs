using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Users;
using ShopClient.Models;
using System.Net.Http.Headers;

namespace ShopClient.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;

        public UsersController(IConfiguration configuration, INotyfService toastNotification)
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

        public async Task<List<User>> GetUsers()
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                try
                {
                    var token = HttpContext.Session.GetString("AuthToken");
                    var tokenAuth = "Bearer " + token;

                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Users", RestSharp.Method.Get);
                    requesrUrl.AddHeader("content-type", "application/json");
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                    var response = await client.ExecuteAsync(requesrUrl);
                    var products = JsonConvert.DeserializeObject<List<User>>(response.Content);
                    if (products != null)
                    {

                        return products;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return null;

        }

        public async Task<List<Role>> GetRoles()
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Roles", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                var response = await client.ExecuteAsync(requesrUrl);
                var roles = JsonConvert.DeserializeObject<List<Role>>(response.Content);
                return roles;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> Index(int currentPage)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var session = HttpContext.Session.GetString("currentuser");
                if (session != null)
                {
                    var currentUser = JsonConvert.DeserializeObject<User>(session);
                    ViewData["Name"] = currentUser.FullName;
                    ViewData["Role"] = currentUser.RoleId;

                }

                var users = GetUsers().Result;
                if (users != null)
                {
                    ViewData["NumberOfPages"] = users.Count / 6;

                    users = users.Skip(6 * currentPage).Take(6).ToList();

                    ViewData["currentPage"] = currentPage;
                    return View(users);
                }
            }


            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

        public async Task<ActionResult> Details(int userId)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;

                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users/" + userId, RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var response = await client.ExecuteAsync(requesrUrl);
                var userD = JsonConvert.DeserializeObject<User>(response.Content);
                if (userD != null)
                {

                    return View(userD);
                }
            }


            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

        public async Task<ActionResult> Edit(int userId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var tokenAuth = "Bearer " + token;

            RestClient client = new RestClient(ApiPort);
            var requesrUrl = new RestRequest("api/Users/" + userId, RestSharp.Method.Get);
            requesrUrl.AddHeader("content-type", "application/json");
            requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
            var response = await client.ExecuteAsync(requesrUrl);
            var user = JsonConvert.DeserializeObject<User>(response.Content);
            if (user != null)
            {
                var listRole = GetRoles();
                ViewData["RoleId"] = new SelectList(listRole.Result.ToList(), "RoleId", "RoleName");
                return View(user);

            }
            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();

        }

        //POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int userId, List<IFormFile> files, [Bind("UserId,UserName,Password,FullName,AvatarImage,Address,BirthDay,Gender,Email,Phone,RoleId")] UserRequest request)
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
                filePaths.Add(request.AvatarImage);
            }

            var token = HttpContext.Session.GetString("AuthToken");
            var tokenAuth = "Bearer " + token;
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users/{userId}", RestSharp.Method.Put);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var body = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = request.Password,
                    FullName = request.FullName,
                    Address = request.Address,
                    AvatarImage = filePaths[0],
                    BirthDay = request.BirthDay,
                    Gender = request.Gender,
                    Phone = request.Phone,
                    RoleId = request.RoleId,
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _toastNotification.Success("Cập nhật người dùng thành công !");
                    return RedirectToAction("Index", "Users");

                }


            }
            catch (Exception)
            {

                throw;
            }
            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

        public async Task<ActionResult> Delete(int userId)
        {
            var user = GetCurrentUser().Result;
            if (user.RoleId == 1)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;
                try
                {
                    RestClient client = new RestClient(ApiPort);
                    var requesrUrl = new RestRequest($"api/Users/{userId}", RestSharp.Method.Delete);
                    requesrUrl.AddHeader("content-type", "application/json");
                    requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                    var response = await client.ExecuteAsync(requesrUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        _toastNotification.Success("Xóa người dùng thành công !");
                        return RedirectToAction("Index", "Users");

                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }

            _toastNotification.Error("Xóa người dùng thất bại !");
            return RedirectToAction("Index", "Users");

        }


        public async Task<ActionResult> Profile(int userId)
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
                return View(user);

            }
            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(int userId, List<IFormFile> files, [Bind("UserId,UserName,Password,FullName,AvatarImage,Address,BirthDay,Gender,Email,Phone,RoleId")] UserRequest request)
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
                filePaths.Add(request.AvatarImage);
            }

            var token = HttpContext.Session.GetString("AuthToken");
            var tokenAuth = "Bearer " + token;
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users/{userId}", RestSharp.Method.Put);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var body = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = request.Password,
                    FullName = request.FullName,
                    Address = request.Address,
                    AvatarImage = filePaths[0],
                    BirthDay = request.BirthDay,
                    Gender = request.Gender,
                    Phone = request.Phone,
                    RoleId = request.RoleId,
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _toastNotification.Success("Cập nhật người dùng thành công !");
                    string refererUrl = Request.Headers["Referer"].ToString();
                    return Redirect(refererUrl);
                }


            }
            catch (Exception)
            {

                throw;
            }
            _toastNotification.Error("Bạn không có quyền truy cập trang này");
            return View();
        }

    }
}
