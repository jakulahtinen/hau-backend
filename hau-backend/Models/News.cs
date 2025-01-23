using System.ComponentModel.DataAnnotations.Schema;

namespace hau_backend.Models
{
    public class News
    {
        public int Id { get; set; } // Pääavain
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        public byte[]? ImageData { get; set; }

        [NotMapped] //EI tallenneta tietokantaan
        public string? ImageDataBase64 { get; set; }
    }
}