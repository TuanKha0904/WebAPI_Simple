using WebAPI_Simple.Models.Domain;

namespace WebAPI_Simple.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; } 
        public string? Fullname { get; set; }
    }

    public class AuthorNoIdDTO
    {
        public string? Fullname { get; set; }
    }
}
