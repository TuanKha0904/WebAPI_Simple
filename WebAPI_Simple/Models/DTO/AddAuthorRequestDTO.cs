using WebAPI_Simple.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Simple.Models.DTO
{
    public class AddAuthorRequestDTO
    {
        [Required (ErrorMessage = "Please enter Fullname for Author!!")]
        [MaxLength(50, ErrorMessage ="Max length for name of aithor is 50 characters")]
        public string? FullName { get; set; }
    }
}
