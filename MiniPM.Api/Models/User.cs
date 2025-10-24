using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniPM.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        public byte[] PasswordHash { get; set; } = null!;

        [Required]
        public byte[] PasswordSalt { get; set; } = null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
