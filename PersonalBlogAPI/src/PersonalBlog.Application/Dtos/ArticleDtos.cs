namespace PersonalBlog.Application.Dtos
{
    public sealed record ArticleDto(string HashId, string Title, string Content, DateTime CreatedAt, DateTime UpdatedAt);

    public sealed record ArticleWithTagsDto(string HashId, string Title, string Content, ICollection<TagDto> Tags, DateTime CreatedAt, DateTime UpdatedAt);
}
