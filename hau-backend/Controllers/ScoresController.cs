using hau_backend.Data;
using Microsoft.AspNetCore.Mvc;
using hau_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace hau_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScoresController(AppDbContext context)
        {
            _context = context;
        }


        //GET
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<Scores>>> GetScores()
        {
            return await _context.Scores.ToListAsync();
        }


        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Scores>> GetScoresById(int id)
        {
            var scoresItem = await _context.Scores.FindAsync(id);
            if (scoresItem == null)
            {
                return NotFound();
            }
            return Ok(scoresItem);
        }


        //POST
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> CreateScores([FromBody] Scores scores)
        {
            if (scores == null || string.IsNullOrWhiteSpace(scores.Title) || string.IsNullOrWhiteSpace(scores.Content))
            {
                return BadRequest("Kaikki kentät ovat pakollisia.");
            }
            _context.Scores.Add(scores);
            await _context.SaveChangesAsync();
            return Ok(scores);
        }


        //PUT
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateScores(int id, Scores updatedScores)
        {
            var scores = await _context.Scores.FindAsync(id);
            if (scores == null) return NotFound();

            scores.Title = updatedScores.Title;
            scores.Content = updatedScores.Content;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        //DELETE
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteScores(int id)
        {
            var scores = await _context.Scores.FindAsync(id);

            if (scores == null)
                return NotFound();

            _context.Scores.Remove(scores);
            await _context.SaveChangesAsync();

            return NoContent();
        }   
    }
}