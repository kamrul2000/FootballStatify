using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Dto;
using MyApp.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeamsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.Include(t => t.Players).ToListAsync();
        }

        // GET: api/teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.Include(t => t.Players)
                                           .FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            return team;
        }

        // POST: api/teams
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(CreateTeamDto dto)
        {
            var team = new Team
            {
                Name = dto.Name,
                Coach = dto.Coach,
                FoundingYear = dto.FoundingYear
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok(team);
        }


        // DELETE: api/teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
