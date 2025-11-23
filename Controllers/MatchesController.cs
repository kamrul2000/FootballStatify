using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Dto;
using MyApp.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            return await _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .ToListAsync();
        }

        // GET: api/matches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null) return NotFound();

            return match;
        }

        // POST: api/matches
        [HttpPost]
        public async Task<ActionResult<Match>> PostMatch(CreateMatchDto dto)
        {
            var match = new Match
            {
                Title = dto.Title,
                TeamAId = dto.TeamAId,
                TeamBId = dto.TeamBId,
                MatchDate = dto.MatchDate,
                Venue = dto.Venue
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            // Load teams to include in the response
            var createdMatch = await _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .FirstOrDefaultAsync(m => m.Id == match.Id);

            return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, createdMatch);
        }



        // DELETE: api/matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null) return NotFound();

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
