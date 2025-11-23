using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hau_backend.Data;
using hau_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace hau_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public NewsController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            return await _context.News.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
            var newsItem = await _context.News.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return Ok(newsItem);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNews([FromBody] News news)
        {
            if (news == null || string.IsNullOrWhiteSpace(news.Title) || string.IsNullOrWhiteSpace(news.Content))
            {
                return BadRequest("Kaikki kentät ovat pakollisia.");
            }

            if (!string.IsNullOrEmpty(news.ImageDataBase64))
            {
                try
                {
                    // 1. Convert Base64 to Stream
                    var imageBytes = Convert.FromBase64String(news.ImageDataBase64);
                    using var stream = new MemoryStream(imageBytes);

                    // 2. Connect to Azure
                    string connectionString = _configuration.GetConnectionString("AzureBlobStorage")
                        ?? throw new InvalidOperationException("AzureBlobStorage connection string is missing or null.");

                    var blobServiceClient = new BlobServiceClient(connectionString);
                    var containerClient = blobServiceClient.GetBlobContainerClient("hau-images");

                    // Ensure container exists
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                    // 3. Create unique filename
                    string fileName = $"news-{Guid.NewGuid()}.jpg";
                    var blobClient = containerClient.GetBlobClient(fileName);

                    // 4. Upload
                    await blobClient.UploadAsync(stream, true);

                    // 5. Save URL to Model
                    news.ImageUrl = blobClient.Uri.ToString();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Kuvan lataus epäonnistui: {ex.Message}");
                }
            }

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return Ok(news);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateNews(int id, News updatedNews)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return NotFound();

            news.Title = updatedNews.Title;
            news.Content = updatedNews.Content;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news == null)
                return NotFound();

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}