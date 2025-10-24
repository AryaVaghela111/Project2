using System.ComponentModel.DataAnnotations;

namespace MiniPM.Api.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = null!;

        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskDto
    {
        [Required]
        public string Title { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
