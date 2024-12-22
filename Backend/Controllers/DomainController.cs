using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.Protocols;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DomainController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("status")]
        public IActionResult CheckDomainStatus()
        {
            var ldapConfig = _configuration.GetSection("LDAP");
            var server = ldapConfig["Server"];
            var portString = ldapConfig["Port"];
            var username = ldapConfig["Username"];
            var password = ldapConfig["Password"];
            var port = int.TryParse(portString, out var parsedPort) ? parsedPort : 389;

            Console.WriteLine($"LDAP Server: {server}");
            Console.WriteLine($"LDAP Port: {port}");
            Console.WriteLine($"LDAP Username: {username}");

            try
            {
                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(server, port)))
                {
                    connection.Credential = new System.Net.NetworkCredential(username, password);
                    connection.Bind();
                }

                Console.WriteLine("LDAP bind successful.");
                return Ok(new { Status = "Connected" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LDAP bind failed: {ex.Message}");
                return Ok(new { Status = "Disconnected", Error = ex.Message });
            }
        }
    }
}
