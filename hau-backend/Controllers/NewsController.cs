using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hau_backend.Data;
using hau_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace hau_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            return await _context.News.ToListAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
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
                    news.ImageData = Convert.FromBase64String(news.ImageDataBase64);
                }
                catch (FormatException)
                {
                    return BadRequest("Virheellinen kuvan Base64-data.");
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