using System.ComponentModel.DataAnnotations;

namespace WebAPI_Simple.Models.Domain
{
    public class Books_Authors
    {
        public int Id { get; set; } 
        public int BookId { get; set; }

        // Navigation Properties - One book has many books-authors
        public Books? Books { get; set; }
        public int AuthorId { get; set; }

        // Navigation Properties - One author has many books-authors
        public Authors? Authors { get; set; }

    }
}
