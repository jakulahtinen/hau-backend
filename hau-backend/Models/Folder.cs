using System.ComponentModel.DataAnnotations;

namespace hau_backend.Models
{
    public class Folder
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [System.Text.Json.Serialization.JsonIgnore]
        public List<Picture>? Pictures { get; set; }
    }
}
