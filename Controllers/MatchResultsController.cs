using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Dto;
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
        [HttpPost]
        public async Task<ActionResult<MatchResult>> PostMatchResult(CreateMatchResultDto dto)
        {
            var matchResult = new MatchResult
            {
                MatchId = dto.MatchId,
                TeamAGoals = dto.TeamAGoals,
                TeamBGoals = dto.TeamBGoals,
            };

            // Auto-calculate winner
            matchResult.Winner = matchResult.TeamAGoals > matchResult.TeamBGoals ? "Team A" :
                                 matchResult.TeamBGoals > matchResult.TeamAGoals ? "Team B" : "Draw";

            _context.MatchResults.Add(matchResult);
            await _context.SaveChangesAsync();

            // Add PlayerStats for scorers
            foreach (var scorer in dto.Scorers)
            {
                var playerStat = new PlayerStat
                {
                    PlayerId = scorer.PlayerId,
                    MatchId = dto.MatchId,
                    Goals = scorer.Goals
                };
                _context.PlayerStats.Add(playerStat);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatchResult), new { id = matchResult.Id }, matchResult);
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
