using hau_backend.Data;
using hau_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace hau_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PicturesController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
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

            try
            {
                // 1. Connect to Azure
                string connectionString = _configuration.GetConnectionString("AzureBlobStorage")
                        ?? throw new InvalidOperationException("AzureBlobStorage connection string is missing or null.");

                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient("hau-images");

                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                // 2. Convert & Upload
                var imageBytes = Convert.FromBase64String(picture.ImageDataBase64);
                using var stream = new MemoryStream(imageBytes);

                string fileName = $"gallery-{Guid.NewGuid()}.jpg";
                var blobClient = containerClient.GetBlobClient(fileName);

                await blobClient.UploadAsync(stream, true);

                // 3. Save URL
                picture.ImageUrl = blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                return BadRequest($"Virhe tallennettaessa kuvaa: {ex.Message}");
            }
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