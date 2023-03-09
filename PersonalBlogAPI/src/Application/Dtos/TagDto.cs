namespace Application.Dtos
{
    public sealed record TagDto(string HashId, string Name, DateTime CreatedAt, DateTime LastModifiedAt);

    public sealed record TagWithArticlesDto(string HashId, string Name, ICollection<ArticleDto> Articles, DateTime CreatedAt, DateTime LastModifiedAt);
}
