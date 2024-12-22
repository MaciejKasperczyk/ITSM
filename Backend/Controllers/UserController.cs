using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/user/current
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            // Symulujemy, że zawsze zwracamy użytkownika z ID 1
            var user = _context.Users.FirstOrDefault(u => u.Id == 1);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(user);
        }
    }
}
