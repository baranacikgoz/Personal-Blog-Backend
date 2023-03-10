namespace Application.Dtos
{
    public sealed record TagDto(string Id, string Name, DateTime CreatedAt, DateTime LastModifiedAt);

    public sealed record TagWithArticlesDto(string Id, string Name, ICollection<ArticleDto> Articles, DateTime CreatedAt, DateTime LastModifiedAt);
}
