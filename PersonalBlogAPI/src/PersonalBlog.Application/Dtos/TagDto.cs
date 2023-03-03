namespace PersonalBlog.Application.Dtos
{
    public sealed record TagDto(string HashId, string Name);

    public sealed record TagWithArticlesDto(string HashId, string Name, ICollection<ArticleDto> Articles);
}
