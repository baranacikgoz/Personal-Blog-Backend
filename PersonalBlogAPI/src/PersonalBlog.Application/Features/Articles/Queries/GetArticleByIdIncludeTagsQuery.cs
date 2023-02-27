using Mapster;
using PersonalBlog.Application.Dtos;
using PersonalBlog.Application.Exceptions;
using PersonalBlog.Application.Features.Abstractions;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Application.Interfaces.Repository.ReadRepositories;
using PersonalBlog.Application.Wrappers;
using PersonalBlog.Domain.Entities;

namespace PersonalBlog.Application.Features.Articles.Queries;

public sealed record GetArticleByIdIncludeTagsQuery(string Id) : IQuery<BaseResponse<ArticleWithTagsDto>>;

internal sealed class GetArticleByIdIncludeTagsQueryHandler : IQueryHandler<GetArticleByIdIncludeTagsQuery, BaseResponse<ArticleWithTagsDto>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IHashIdService _hashIdService;

    public GetArticleByIdIncludeTagsQueryHandler(IArticleRepository articleRepository, IHashIdService hashIdService) => (_articleRepository, _hashIdService) = (articleRepository, hashIdService);

    public async Task<BaseResponse<ArticleWithTagsDto>> Handle(GetArticleByIdIncludeTagsQuery request, CancellationToken cancellationToken)
    {
        var id = _hashIdService.Decode(request.Id);

        var article = await _articleRepository.GetByIdIncludeArticleTags(id, cancellationToken);
        _ = article ?? throw new EntityNotFoundException(request.Id, typeof(Article).Name);

        var dto = article.Adapt<ArticleWithTagsDto>();

        return BaseResponse<ArticleWithTagsDto>.FromSuccess(dto);
    }
}