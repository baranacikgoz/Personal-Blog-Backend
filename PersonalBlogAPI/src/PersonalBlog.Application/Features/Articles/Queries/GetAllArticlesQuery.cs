﻿using Mapster;
using PersonalBlog.Application.Dtos;
using PersonalBlog.Application.Features.Abstractions;
using PersonalBlog.Application.Interfaces.Repository.ReadRepositories;
using PersonalBlog.Application.Wrappers;

namespace PersonalBlog.Application.Features.Articles.Queries;

public sealed record GetAllArticlesQuery() : IQuery<BaseResponse<IReadOnlyCollection<ArticleDto>>>;

internal sealed class GetAllArticlesQueryHandler : IQueryHandler<GetAllArticlesQuery, BaseResponse<IReadOnlyCollection<ArticleDto>>>
{
    private readonly IArticleRepository _articleRepository;

    public GetAllArticlesQueryHandler(IArticleRepository articleRepository) => (_articleRepository) = (articleRepository);

    public async Task<BaseResponse<IReadOnlyCollection<ArticleDto>>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = await _articleRepository.GetAllAsync(cancellationToken);

        var dto = articles.Adapt<IReadOnlyCollection<ArticleDto>>();

        return BaseResponse<IReadOnlyCollection<ArticleDto>>.FromSuccess(dto);
    }
}