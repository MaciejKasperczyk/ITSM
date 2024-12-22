using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tickets
        [HttpGet]
        public IActionResult GetTickets()
        {
            var tickets = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.AssignedTo)
                .ToList();

            return Ok(tickets);
        }

        // GET: api/tickets/{id}/details
        [HttpGet("{id}/details")]
        public IActionResult GetTicketDetails(int id)
        {
            var ticket = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.AssignedTo)
                .Include(t => t.Messages)
                .FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound(new { Message = "Ticket not found" });
            }

            return Ok(ticket);
        }

        // POST: api/tickets
        [HttpPost]
        public IActionResult CreateTicket([FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data provided", Errors = ModelState.Values });
            }

            // Sprawdzamy czy użytkownik jest dostępny w bazie danych
            var loggedInUser = _context.Users.FirstOrDefault(u => u.Id == ticket.UserId);
            if (loggedInUser == null)
            {
                return BadRequest(new { Message = "User not found" });
            }

            ticket.UserId = loggedInUser.Id;
            ticket.Status = ticket.Status ?? "Open";
            ticket.Priority = ticket.Priority ?? "Medium";
            ticket.CreatedDate = DateTime.UtcNow;

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTicketDetails), new { id = ticket.Id }, ticket);
        }

        // PUT: api/tickets/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest(new { Message = "Ticket ID mismatch" });
            }

            var existingTicket = _context.Tickets.Include(t => t.User).FirstOrDefault(t => t.Id == id);
            if (existingTicket == null)
            {
                return NotFound(new { Message = "Ticket not found" });
            }

            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;
            existingTicket.Status = ticket.Status;
            existingTicket.Priority = ticket.Priority;
            existingTicket.Type = ticket.Type;
            existingTicket.AssignedToId = ticket.AssignedToId;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/tickets/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound(new { Message = "Ticket not found" });
            }

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
