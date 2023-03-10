using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Mapster;

namespace Application.Mappings
{
    public static class MapsterProfile
    {
        public static void AddConfigs(IHashIdService hashIdService)
        {
            _ = TypeAdapterConfig<Article, ArticleDto>
                .NewConfig()
                .Map(dest => dest.Id, src => hashIdService.Encode(src.Id));

            _ = TypeAdapterConfig<Article, ArticleWithTagsDto>
                .NewConfig()
                .Map(dest => dest.Id, src => hashIdService.Encode(src.Id));

            _ = TypeAdapterConfig<Tag, TagDto>
                .NewConfig()
                .Map(dest => dest.Id, src => hashIdService.Encode(src.Id));

            _ = TypeAdapterConfig<Tag, TagWithArticlesDto>
                .NewConfig()
                .Map(dest => dest.Id, src => hashIdService.Encode(src.Id));
        }
    }
}
