namespace Backend.Models
{
    public class LdapConfiguration
    {
        public string Server { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}