using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesShoppingOnline.DTO.Request.Users;
using ShoesShoppingOnline.Models;
using ShoesShoppingOnline.Repositories;
using System.Security.Claims;

namespace ShoesShoppingOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShoesShoppingOnlineContext _context;
        private readonly IUserRepository repo;

        public UsersController(ShoesShoppingOnlineContext context, IUserRepository repo)
        {
            _context = context;
            this.repo = repo;
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }
        [Authorize]
        [HttpGet("get-current-user")]
        public UserRequest GetUserContext()
        {
            return GetCurrentUser();
        }
        [Authorize]
        [HttpPost("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordRequest request)
        {
            var user = repo.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            repo.ChangePassword(userId, request.newPassword);
            return NoContent();
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

        // GET: api/Users/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserRequest request)
        {
            var user = _context.Users.Find(id);
            user.FullName = request.FullName;
            user.UserName = request.UserName;
            user.Phone = request.Phone;
            user.Email = request.Email;
            user.Address = request.Address;
            user.Password = request.Password;
            user.AvatarImage = request.AvatarImage;
            user.BirthDay = request.BirthDay;
            user.Gender = request.Gender;
            user.RoleId = request.RoleId;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserRequest request)
        {
            var user = new User
            {
                FullName = request.FullName,
                UserName = request.UserName,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                Password = request.Password,
                AvatarImage = request.AvatarImage,
                BirthDay = request.BirthDay,
                Gender = request.Gender,
                RoleId = request.RoleId,

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        [Authorize]
        [HttpGet("get-user-by-username/{username}")]
        public async Task<ActionResult<User>> GetUserByUserName(string username)
        {
            var user = repo.GetUserByUsername(username);
            if (user == null)
            {
                return null;
            }
            return user;

        }
    }
}
