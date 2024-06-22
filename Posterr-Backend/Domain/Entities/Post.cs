
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {
        [MaxLength(777)]
        public string? Content { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int? OriginalPostId { get; set; }

        [ForeignKey("OriginalPostId")]
        public virtual Post? OriginalPost { get; set; }
    }
}
