using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [DefaultValue("getdate()")]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
