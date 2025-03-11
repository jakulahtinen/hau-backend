using System.ComponentModel.DataAnnotations.Schema;

namespace hau_backend.Models
{
    public class Picture
    {
        public int Id { get; set; } //Main key
        public string Title { get; set; } = string.Empty; // Picture description
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow; 
        public byte[]? ImageData { get; set; }

        [NotMapped] //NOT saved to database
        public string? ImageDataBase64 { get; set; }
    }
}
