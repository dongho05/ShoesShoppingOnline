﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesShoppingOnline.DTO.Request.Roles;
using ShoesShoppingOnline.Models;

namespace ShoesShoppingOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ShoesShoppingOnlineContext _context;

        public RolesController(ShoesShoppingOnlineContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            return await _context.Roles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleRequest request)
        {
            var role = _context.Roles.Find(id);
            role.RoleName = request.RoleName;

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(RoleRequest request)
        {
            var role = new Role
            {
                RoleName = request.RoleName
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return (_context.Roles?.Any(e => e.RoleId == id)).GetValueOrDefault();
        }
    }
}