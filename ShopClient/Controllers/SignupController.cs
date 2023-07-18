using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using ShopClient.DTO.Request.Users;
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
