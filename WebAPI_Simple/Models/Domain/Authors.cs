using System.ComponentModel.DataAnnotations;

namespace WebAPI_Simple.Models.Domain
{
    public class Authors
    {
        public int Id { get; set; }

        public string? Fullname { get; set; }

        // Navigation Properties - One Author has many books-authors
        public List<Books_Authors>? Books_Authors { get; set; }
    }
}
