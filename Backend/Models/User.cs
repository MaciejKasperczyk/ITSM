using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "User"; // Domyślna rola użytkownika

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Data utworzenia

        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty; // Pełne imię i nazwisko użytkownika

        public DateTime? LastLoginDate { get; set; } // Data ostatniego logowania (opcjonalna)
    }
}
