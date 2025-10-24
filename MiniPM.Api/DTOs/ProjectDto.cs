using System.ComponentModel.DataAnnotations;

namespace MiniPM.Api.DTOs
{
    public class CreateProjectDto
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
