using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebAPI_Simple.Models.Domain
{
    public class Books
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateRead { get; set; }

        public int Rate { get; set; }
        public string? Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // Navigation Properties - One publishers has many books 
        public int PublisherID { get; set; }
        public Publishers? Publishers { get; set; }

        // Navigation Properties - One book has many books-authors
        public List<Books_Authors>? Books_Authors { get; set; }

    }
}
