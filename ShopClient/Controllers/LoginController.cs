using Microsoft.AspNetCore.Mvc;
using RestSharp;
using ShopClient.DTO.Request.Login;
using System.Text.Json;

namespace ShopClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
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
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception)
            {

                throw;
            }



            return Ok("Tai khoan hoac mat khau ban nhap sai !!!");
        }

    }
}
