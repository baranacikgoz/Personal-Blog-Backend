using Mapster;
using Application.Dtos;
using Application.Features.Abstractions;
using Application.Interfaces.Repository.ReadRepositories;
using Application.Wrappers;

namespace Application.Features.Articles.Queries;

public sealed record GetAllArticlesQuery() : IQuery<BaseResponse<IReadOnlyCollection<ArticleDto>>>;

internal sealed class GetAllArticlesQueryHandler : IQueryHandler<GetAllArticlesQuery, BaseResponse<IReadOnlyCollection<ArticleDto>>>
{
    private readonly IArticleRepository _articleRepository;

    public GetAllArticlesQueryHandler(
        IArticleRepository articleRepository
        )
    {
        _articleRepository = articleRepository;
    }

    public async Task<BaseResponse<IReadOnlyCollection<ArticleDto>>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Domain.Entities.Article> articles = await _articleRepository.GetAllAsync(cancellationToken);

        IReadOnlyCollection<ArticleDto> dto = articles.Adapt<IReadOnlyCollection<ArticleDto>>();

        return BaseResponse<IReadOnlyCollection<ArticleDto>>.FromSuccess(dto);
    }
}