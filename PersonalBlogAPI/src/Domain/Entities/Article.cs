using Domain.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Article : AuditableBaseEntity
    {
        public const int TitleMaxLength = 50;
        public const int ContentMaxLength = 500;

        [Required]
        [MaxLength(TitleMaxLength)]
        [Column(nameof(Title))]
        public required string Title { get; set; } = null!;

        [Required]
        [MaxLength(ContentMaxLength)]
        [Column(nameof(Content))]
        public required string Content { get; set; } = null!;

        public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new Collection<ArticleTag>();
    }
}
