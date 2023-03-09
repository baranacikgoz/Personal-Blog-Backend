namespace Application.Dtos
{
    public sealed record ArticleDto(string HashId, string Title, string Content, DateTime CreatedAt, DateTime LastModifiedAt);

    public sealed record ArticleWithTagsDto(string HashId, string Title, string Content, ICollection<TagDto> Tags, DateTime CreatedAt, DateTime LastModifiedAt);
}
