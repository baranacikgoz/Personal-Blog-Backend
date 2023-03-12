using Domain.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Tag : AuditableBaseEntity
    {
        public const int NameMaxLength = 50;

        [Required]
        [MaxLength(NameMaxLength)]
        public required string Name { get; set; } = null!;

        public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new Collection<ArticleTag>();
    }
}
