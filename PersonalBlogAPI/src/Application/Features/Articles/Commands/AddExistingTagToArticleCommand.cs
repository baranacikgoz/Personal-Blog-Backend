using FluentValidation;
using Mapster;
using Application.Dtos;
using Application.Exceptions;
using Application.Features.Abstractions;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.ReadRepositories;
using Application.Wrappers;
using Domain.Entities;

namespace Application.Features.Articles.Commands;

public sealed record AddExistingTagToArticleCommand(string ArticleId, string TagId) : ICommand<BaseResponse<ArticleWithTagsDto>>;

public class AddExistingTagToArticleCommandValidator : AbstractValidator<AddExistingTagToArticleCommand>
{
    public AddExistingTagToArticleCommandValidator(IArticleRepository articleRepository, ITagRepository tagRepository, IHashIdService hashIdService)
    {
        RuleFor(request => request.ArticleId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MustAsync(async (id, cancellationToken) => await articleRepository.ExistsAsync(hashIdService.Decode(id), cancellationToken))
            .WithMessage("{PropertyName} does not exist.");

        RuleFor(request => request.TagId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MustAsync(async (id, cancellationToken) => await tagRepository.ExistsAsync(hashIdService.Decode(id), cancellationToken))
            .WithMessage("{PropertyName} does not exist.");
    }
}

internal sealed class AddExistingTagToArticleCommandHandler : ICommandHandler<AddExistingTagToArticleCommand, BaseResponse<ArticleWithTagsDto>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IHashIdService _hashIdService;

    public AddExistingTagToArticleCommandHandler(
        IArticleRepository articleRepository,
        ITagRepository tagRepository,
        IHashIdService hashIdService
        )
    {
        _articleRepository = articleRepository;
        _tagRepository = tagRepository;
        _hashIdService = hashIdService;
    }

    public async Task<BaseResponse<ArticleWithTagsDto>> Handle(AddExistingTagToArticleCommand request, CancellationToken cancellationToken)
    {
        int articleId = _hashIdService.Decode(request.ArticleId);
        int tagId = _hashIdService.Decode(request.TagId);

        var article = await _articleRepository.GetByIdIncludeArticleTags(articleId, cancellationToken);

        _ = article ?? throw new EntityNotFoundException(request.ArticleId, typeof(Article).Name);

        var tag = await _tagRepository.GetByIdIncludeArticleTags(tagId, cancellationToken);

        _ = tag ?? throw new EntityNotFoundException(request.TagId, typeof(Tag).Name);

        article = await _articleRepository.AddExistingTagToArticle(article, tag, cancellationToken);

        var dto = article.Adapt<ArticleWithTagsDto>();

        return BaseResponse<ArticleWithTagsDto>.FromSuccess(dto);
    }
}