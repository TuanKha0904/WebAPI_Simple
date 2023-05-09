using WebAPI_Simple.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Simple.Models.DTO
{
    public class AddBookRequestDTO
    {
        [Required (ErrorMessage = "Please enter title for book!!")]
        [MaxLength(100, ErrorMessage = "Title just use 100 characters")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }

        [Range (1, 5, ErrorMessage = "Rate limit between from 1 to 5")]
        public int? Rate { get; set; }
        public string? Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // Navigation properties
        public int PublisherID { get; set; }
        public List<int>? AuthorIds { get; set; }
    }
}
