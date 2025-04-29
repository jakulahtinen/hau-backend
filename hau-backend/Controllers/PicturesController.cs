using hau_backend.Data;
using hau_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hau_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PicturesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPictures()
        {
            return await _context.Pictures
                .OrderByDescending(p => p.UploadedAt)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> GetPictureById(int id)
        {
            var picture = await _context.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }
            return Ok(picture);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPicture([FromBody] Picture picture)
        {
            if (string.IsNullOrEmpty(picture.ImageDataBase64))
            {
                return BadRequest("Kuva on pakollinen.");
            }

            picture.ImageData = Convert.FromBase64String(picture.ImageDataBase64);
            _context.Pictures.Add(picture);

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPictures), new { id = picture.Id }, picture);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            var picture = await _context.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }
            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCaption(int id, [FromBody] Picture picture)
        {
            if (id != picture.Id)
            {
                return BadRequest();
            }
            var pictureToUpdate = await _context.Pictures.FindAsync(id);
            if (pictureToUpdate == null)
            {
                return NotFound();
            }
            pictureToUpdate.Title = picture.Title;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}