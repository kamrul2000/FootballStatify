using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchResultsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchResultsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/matchresults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchResult>>> GetMatchResults()
        {
            return await _context.MatchResults
                .Include(r => r.Match)
                .ThenInclude(m => m.TeamA)
                .Include(r => r.Match)
                .ThenInclude(m => m.TeamB)
                .ToListAsync();
        }

        // GET api/matchresults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchResult>> GetMatchResult(int id)
        {
            var result = await _context.MatchResults
                .Include(r => r.Match)
                .ThenInclude(m => m.TeamA)
                .Include(r => r.Match)
                .ThenInclude(m => m.TeamB)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (result == null) return NotFound();

            return result;
        }

        // POST api/matchresults
        [HttpPost]
        public async Task<ActionResult<MatchResult>> PostMatchResult(MatchResult result)
        {
            // Auto calculate winner
            if (result.TeamAGoals > result.TeamBGoals)
                result.Winner = "Team A";
            else if (result.TeamBGoals > result.TeamAGoals)
                result.Winner = "Team B";
            else
                result.Winner = "Draw";

            _context.MatchResults.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatchResult), new { id = result.Id }, result);
        }



        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatchResult(int id)
        {
            var result = await _context.MatchResults.FindAsync(id);
            if (result == null) return NotFound();

            _context.MatchResults.Remove(result);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
