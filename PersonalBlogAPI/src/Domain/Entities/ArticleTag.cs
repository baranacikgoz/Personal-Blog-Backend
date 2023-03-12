using Domain.Abstractions;

namespace Domain.Entities;

public class ArticleTag : AuditableBaseEntity
{
    public required int ArticleId { get; set; }
    public Article Article { get; set; } = null!;
    public required int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
