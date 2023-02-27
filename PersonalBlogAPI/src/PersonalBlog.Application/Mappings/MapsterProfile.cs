using Mapster;
using PersonalBlog.Application.Dtos;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Domain.Abstractions;
using PersonalBlog.Domain.Entities;

namespace PersonalBlog.Application.Mappings;

public class MapsterProfile
{
    private readonly IHashIdService _hashIdService;

    public MapsterProfile(IHashIdService hashIdService)
    {
        _hashIdService = hashIdService;
    }

    public void AddConfigs()
    {
        TypeAdapterConfig<Article, ArticleDto>
            .NewConfig()
            .Map(dest => dest.Id, src => _hashIdService.Encode(src.Id));

        TypeAdapterConfig<Article, ArticleWithTagsDto>
            .NewConfig()
            .Map(dest => dest.Id, src => _hashIdService.Encode(src.Id))
            .Map(dest => dest.Tags, src => src.ArticleTags.Select(at => at.Tag).Adapt<IEnumerable<TagDto>>());

        TypeAdapterConfig<Tag, TagDto>
            .NewConfig()
            .Map(dest => dest.Id, src => _hashIdService.Encode(src.Id));

        TypeAdapterConfig<Tag, TagWithArticlesDto>
            .NewConfig()
            .Map(dest => dest.Id, src => _hashIdService.Encode(src.Id))
            .Map(dest => dest.Articles, src => src.ArticleTags.Select(at => at.Article).Adapt<IEnumerable<ArticleDto>>());
    }
}