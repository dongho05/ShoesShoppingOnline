using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShoesShoppingOnline.Models;
using ShoesShoppingOnline.Repositories;
using System.Text;

namespace ShoesShoppingOnline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ShoesShoppingOnlineContext>();

            builder.Services.AddSingleton<IBrandRepository, BrandRepository>();
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddSingleton<IRoleRepository, RoleRepository>();
            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
            builder.Services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();


            // add configure jwt bearer
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWTSettings:validIssuer"],
            ValidAudience = builder.Configuration["JWTSettings:validIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:securityKey"]))
        };
    });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}