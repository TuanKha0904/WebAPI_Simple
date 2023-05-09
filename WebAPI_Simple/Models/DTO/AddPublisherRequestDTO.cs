using System.ComponentModel.DataAnnotations;

namespace WebAPI_Simple.Models.DTO
{
    public class AddPublisherRequestDTO
    {
        [Required (ErrorMessage = "Please enter name for publisher")]
        [MaxLength (255, ErrorMessage = "Max lengh for Name is 255 characters")]
        public string? Name { get; set; }

    }

}
