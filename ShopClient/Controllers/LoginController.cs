using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopClient.DTO.Request.Login;
using ShopClient.DTO.Request.Users;
using System.Text.Json;

namespace ShopClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private readonly INotyfService _toastNotification;
        public LoginController(IConfiguration configuration, INotyfService toastNotification)
        {
            this.configuration = configuration;
            _toastNotification = toastNotification;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Login", RestSharp.Method.Post);
                requesrUrl.AddHeader("content-type", "application/json");
                var body = new LoginRequest
                {
                    username = request.username,
                    password = request.password
                };
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                requesrUrl.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(requesrUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("AuthToken", response.Content.ToString());

                    var currentUser = await GetCurrentUser();
                    var currentUserJson = JsonConvert.SerializeObject(currentUser);
                    HttpContext.Session.SetString("currentuser", currentUserJson);

                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception)
            {

                throw;
            }



            return Ok("Tai khoan hoac mat khau ban nhap sai !!!");
        }


        public async Task<UserRequest> GetCurrentUser()
        {
            if (HttpContext.Session.GetString("AuthToken") != null)
            {

                var token = HttpContext.Session.GetString("AuthToken");
                var tokenAuth = "Bearer " + token;


                RestClient client = new RestClient(ApiPort);
                var requesrUrl = new RestRequest($"api/Users/get-current-user", RestSharp.Method.Get);
                requesrUrl.AddHeader("content-type", "application/json");
                requesrUrl.AddParameter("Authorization", tokenAuth.Replace("\"", ""), ParameterType.HttpHeader);
                var response = await client.ExecuteAsync(requesrUrl);
                var user = JsonConvert.DeserializeObject<UserRequest>(response.Content);
                return user;
            }
            return null;
        }




    }
}
