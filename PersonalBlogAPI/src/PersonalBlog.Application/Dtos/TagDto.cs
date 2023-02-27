namespace PersonalBlog.Application.Dtos;

public sealed record TagDto(string Id, string Name);

public sealed record TagWithArticlesDto(string Id, string Name, ICollection<ArticleDto> Articles);