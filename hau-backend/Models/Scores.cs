namespace hau_backend.Models
{
    public class Scores
    {
        public int Id { get; set; } // Main key
        public string Title { get; set; } = string.Empty; 
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    }
}
