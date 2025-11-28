using hau_backend.Data;
using hau_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hau_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoldersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Folders
        // Returns folders AND the URL of the latest image in that folder (for the thumbnail)
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<object>>> GetFolders()
        {
            var folders = await _context.Folders
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.CreatedAt,
                    // Get the image URL of the most recent picture in this folder to use as a cover
                    CoverImageUrl = f.Pictures != null && f.Pictures.Any()
                        ? f.Pictures.OrderByDescending(p => p.UploadedAt).Select(p => p.ImageUrl).FirstOrDefault()
                        : null
                })
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return Ok(folders);
        }

        // POST: api/Folders
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Folder>> CreateFolder(Folder folder)
        {
            if (string.IsNullOrWhiteSpace(folder.Name))
                return BadRequest("Kansion nimi on pakollinen.");

            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFolders), new { id = folder.Id }, folder);
        }

        // DELETE: api/Folders/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            var folder = await _context.Folders.Include(f => f.Pictures).FirstOrDefaultAsync(f => f.Id == id);
            if (folder == null) return NotFound();

            if (folder.Pictures != null && folder.Pictures.Any())
            {
                return BadRequest("Kansio ei ole tyhjä. Poista kuvat ensin.");
            }

            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
