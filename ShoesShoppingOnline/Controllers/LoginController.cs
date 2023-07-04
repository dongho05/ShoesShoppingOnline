using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShoesShoppingOnline.DTO.Request.Login;
using ShoesShoppingOnline.Models;
using ShoesShoppingOnline.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoesShoppingOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository repo;
        private readonly IConfiguration configuration;

        public LoginController(IUserRepository repo, IConfiguration configuration)
        {
            this.repo = repo;
            this.configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var user = AuthenticateUser(request);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return Unauthorized();
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:securityKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Role,user.RoleId.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Surname,user.FullName)

            };

            var token = new JwtSecurityToken(configuration["JWTSettings:validIssuer"], configuration["JWTSettings:validIssuer"], claims, expires: DateTime.Now.AddMinutes(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(LoginRequest request)
        {
            var currentUser = repo.Login(request.UserName, request.Password);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}
