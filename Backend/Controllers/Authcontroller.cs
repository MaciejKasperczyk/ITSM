using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Data;
using Backend.Models;
using System;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public AuthController(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username or password cannot be empty." });
            }

            var ldapConfig = _configuration.GetSection("LDAP");
            var server = ldapConfig["Server"];

            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, server, request.Username, request.Password))
                {
                    if (context.ValidateCredentials(request.Username, request.Password))
                    {
                        var email = request.Username;
                        var username = email.Split('@')[0];

                        // Check if user exists in the database
                        var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
                        if (user == null)
                        {
                            // Create new user
                            var newUser = new User
                            {
                                Username = username,
                                Email = email,
                                FullName = GetFullNameFromAD(context, email) ?? "Unknown User",
                                CreatedAt = DateTime.UtcNow,
                                Role = "User"
                            };

                            _dbContext.Users.Add(newUser);
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            // Update user's last login date
                            user.LastLoginDate = DateTime.UtcNow;
                            _dbContext.SaveChanges();
                        }

                        var token = GenerateJwtToken(email);
                        return Ok(new { token, username = email });
                    }
                    else
                    {
                        return Unauthorized(new { message = "Invalid credentials" });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LDAP login failed: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }

        private string GenerateJwtToken(string username)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string? GetFullNameFromAD(PrincipalContext context, string email)
        {
            try
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        var directoryEntry = result.GetUnderlyingObject() as System.DirectoryServices.DirectoryEntry;
                        if (directoryEntry?.Properties["mail"]?.Value?.ToString() == email)
                        {
                            return directoryEntry.Properties["displayName"]?.Value?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching full name from AD: {ex.Message}");
            }
            return null;
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
