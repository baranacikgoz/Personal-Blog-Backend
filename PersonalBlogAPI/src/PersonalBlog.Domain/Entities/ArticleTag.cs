using PersonalBlog.Domain.Abstractions;

namespace PersonalBlog.Domain.Entities;

public class ArticleTag : AuditableBaseEntity
{
    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}