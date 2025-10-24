using System.ComponentModel.DataAnnotations;

namespace MiniPM.Api.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(1)]
        public string Title { get; set; } = null!;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
