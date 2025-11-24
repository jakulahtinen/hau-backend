using System.ComponentModel.DataAnnotations.Schema;

namespace hau_backend.Models
{
    public class News
    {
        public int Id { get; set; } // Main key
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        // Stores the link to Azure 
        public string? ImageUrl { get; set; }

        [NotMapped]
        public string? ImageDataBase64 { get; set; }
    }
}