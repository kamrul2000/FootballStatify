using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerStatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayerStatsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/playerstats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerStat>>> GetPlayerStats()
        {
            return await _context.PlayerStats
                .Include(ps => ps.Player)
                .Include(ps => ps.Match)
                .ToListAsync();
        }

        // GET: api/playerstats/player/5  (All stats of single player)
        [HttpGet("player/{playerId}")]
        public async Task<ActionResult<IEnumerable<PlayerStat>>> GetStatsByPlayer(int playerId)
        {
            return await _context.PlayerStats
                .Where(ps => ps.PlayerId == playerId)
                .Include(ps => ps.Match)
                .ToListAsync();
        }

        // GET: api/playerstats/match/5 (all stats of a match)
        [HttpGet("match/{matchId}")]
        public async Task<ActionResult<IEnumerable<PlayerStat>>> GetStatsByMatch(int matchId)
        {
            return await _context.PlayerStats
                .Where(ps => ps.MatchId == matchId)
                .Include(ps => ps.Player)
                .ToListAsync();
        }

        // POST: api/playerstats
        [HttpPost]
        public async Task<ActionResult<PlayerStat>> PostPlayerStat(PlayerStat stat)
        {
            _context.PlayerStats.Add(stat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayerStats), new { id = stat.Id }, stat);
        }

        // PUT: api/playerstats/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayerStat(int id, PlayerStat stat)
        {
            if (id != stat.Id) return BadRequest();

            var existing = await _context.PlayerStats.FindAsync(id);
            if (existing == null) return NotFound();

            existing.PlayerId = stat.PlayerId;
            existing.MatchId = stat.MatchId;
            existing.Goals = stat.Goals;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.MatchResults.AnyAsync(r => r.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/playerstats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerStat(int id)
        {
            var stat = await _context.PlayerStats.FindAsync(id);
            if (stat == null) return NotFound();

            _context.PlayerStats.Remove(stat);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/playerstats/totalgoals/3
        [HttpGet("totalgoals/{playerId}")]
        public async Task<ActionResult<int>> GetTotalGoals(int playerId)
        {
            int totalGoals = await _context.PlayerStats
                .Where(ps => ps.PlayerId == playerId)
                .SumAsync(ps => ps.Goals);

            return totalGoals;
        }
    }
}
