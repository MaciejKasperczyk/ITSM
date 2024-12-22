using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Configuration;

namespace Backend.Services
{
    public class LdapSyncService
    {
        private readonly IConfiguration _configuration;

        public LdapSyncService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SyncUsersFromLdap(string loggedInUsername, string loggedInPassword)
        {
            var ldapConfig = _configuration.GetSection("LDAP");
            var server = ldapConfig["Server"];
            var portString = ldapConfig["Port"];
            var port = int.TryParse(portString, out var parsedPort) ? parsedPort : 389;

            Console.WriteLine($"LDAP Connection Details: Server={server}, Port={port}");

            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, server, loggedInUsername, loggedInPassword))
                {
                    Console.WriteLine("LDAP Connection Successful");
                    // Synchronizacja użytkowników (do zaimplementowania)
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LDAP sync failed: {ex.Message}");
                throw;
            }
        }
    }
}
