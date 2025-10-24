using System.ComponentModel.DataAnnotations;

namespace MiniPM.Api.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
