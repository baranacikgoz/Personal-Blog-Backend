namespace PersonalBlog.Application.Dtos;

public sealed record ArticleDto(string Id, string Title, string Content, DateTime CreatedAt, DateTime UpdatedAt);

public sealed record ArticleWithTagsDto(string Id, string Title, string Content, ICollection<TagDto> Tags, DateTime CreatedAt, DateTime UpdatedAt);