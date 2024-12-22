using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Backend.Enums;

namespace Backend.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Open";

        [Required]
        public string Priority { get; set; } = "Medium";

        [Required]
        public TicketType Type { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [IgnoreDataMember] // Ignorujemy podczas deserializacji
        public User User { get; set; } = null!;

        public int? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public User? AssignedTo { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
