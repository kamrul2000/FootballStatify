using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Dto;
using MyApp.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var players = await _context.Players
        .Include(p => p.Team) // Load related Team
        .ToListAsync();
            return players;
        }

        // GET: api/players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players
    .Include(p => p.Team)
    .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null) return NotFound();

            return Ok(player);

        }

        // POST: api/players
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(CreatePlayerDto dto)
        {
            var player = new Player
            {
                Name = dto.Name,
                Position = dto.Position,
                Age = dto.Age,
                TeamId = dto.TeamId
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok(player);

        }     

        // DELETE: api/players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
