using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Users;
using ShopClient.Models;
using System.Text.Json;

namespace ShopClient.Controllers
{
    public class SignupController : Controller
    {

        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;
        public SignupController(IConfiguration configuration, INotyfService toastNotification)
        {
            this.configuration = configuration;
            _toastNotification = toastNotification;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }


        public IActionResult Index()
        {
            return View();
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

        [HttpPost("Signup")]
        public async Task<ActionResult> Signup([Bind("UserId,UserName,Password,FullName,AvatarImage,Address,BirthDay,Gender,Email,Phone,RoleId")] UserRequest request)
        {
            var repeatPass = Request.Form["repeatpassword"];
            if (!String.IsNullOrEmpty(repeatPass))
            {
                if (!repeatPass.Equals(request.Password))
                {
                    _toastNotification.Error("Mật khẩu thứ 2 phải khớp với mật khẩu thứ nhất !");
                    return RedirectToAction("Index", "Signup");
                }
            }
            else
            {
                _toastNotification.Error("Hãy nhập tất cả các trường !");
                return RedirectToAction("Index", "Signup");
            }

            var checkUser = GetUserByUserName(request.UserName).Result;
            if (checkUser != null)
            {
                _toastNotification.Error("Người dùng đã tồn tại !");
                return RedirectToAction("Index", "Login");
            }
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new UserRequest
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = request.Password,
                    FullName = request.FullName,
                    Address = request.Address,
                    AvatarImage = request.AvatarImage,
                    BirthDay = request.BirthDay,
                    Gender = request.Gender,
                    Phone = request.Phone,
                    RoleId = 2
                };
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {

                    _toastNotification.Success("Đăng ký thành công !");
                    return RedirectToAction("Index", "Login");
                }

            }
            catch (Exception)
            {

                throw;
            }
            _toastNotification.Error("Có lỗi xảy ra !");
            return RedirectToAction("Index", "Login");
        }
    }
}
