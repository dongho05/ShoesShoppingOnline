using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesShoppingOnline.DTO.Request.Products;
using ShoesShoppingOnline.DTO.Request.Users;
using ShoesShoppingOnline.Models;
using System.Security.Claims;

namespace ShoesShoppingOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShoesShoppingOnlineContext _context;

        public ProductsController(ShoesShoppingOnlineContext context)
        {
            _context = context;
        }
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //var user = GetCurrentUser();

            return await _context.Products.ToListAsync();
        }

        private UserRequest GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserRequest
                {
                    RoleId = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value),
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    FullName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value

                };
            }
            return null;
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductRequest request)
        {
            var product = _context.Products.Find(id);
            product.ProductName = request.ProductName;
            product.ImageProduct = request.ImageProduct;
            product.IsSaled = request.IsSaled;
            product.Describe = request.Describe;
            product.UnitPrice = request.UnitPrice;
            product.UnitInStock = request.UnitInStock;
            product.BrandId = request.BrandId;
            product.CategoryId = request.CategoryId;
            product.Discount = request.Discount;
            product.IsActivated = request.IsActivated;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductRequest request)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                ImageProduct = request.ImageProduct,
                IsSaled = request.IsSaled,
                IsActivated = request.IsActivated,
                Discount = request.Discount,
                UnitPrice = request.UnitPrice,
                UnitInStock = request.UnitInStock,
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                Describe = request.Describe
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
