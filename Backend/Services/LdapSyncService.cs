using System;
using System.DirectoryServices.Protocols;
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
            var port = ldapConfig["Port"];

            Console.WriteLine($"LDAP Connection Details: Server={server}, Port={port}");

            try
            {
                var credential = new System.Net.NetworkCredential(loggedInUsername, loggedInPassword);
                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(server, int.Parse(port))))
                {
                    connection.Credential = credential;
                    connection.AuthType = AuthType.Negotiate;

                    Console.WriteLine("Attempting to bind to LDAP...");
                    connection.Bind(); // Sprawdzenie połączenia
                    Console.WriteLine("LDAP Connection Successful");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LDAP connection failed: {ex.Message}");
                throw;
            }
        }
    }
}
