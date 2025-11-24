using System.ComponentModel.DataAnnotations.Schema;

namespace hau_backend.Models
{
    public class Picture
    {
        public int Id { get; set; } //Main key
        public string Title { get; set; } = string.Empty; // Picture description
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

       // Stores the link to Azure
        public string? ImageUrl { get; set; }

        // KEEP: To receive input from React
        [NotMapped]
        public string? ImageDataBase64 { get; set; }
    }
}