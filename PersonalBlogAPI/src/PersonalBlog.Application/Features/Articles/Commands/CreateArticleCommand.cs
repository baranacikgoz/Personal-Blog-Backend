﻿using FluentValidation;
using Mapster;
using PersonalBlog.Application.Features.Abstractions;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Application.Interfaces.Repository.ReadRepositories;
using PersonalBlog.Application.Wrappers;
using PersonalBlog.Domain.Entities;

namespace PersonalBlog.Application.Features.Articles.Commands;

public sealed record CreateArticleCommand(string Title, string Content) : ICommand<BaseResponse<string>>;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator(IArticleRepository articleRepository)
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .WithMessage($"{nameof(Article.Title)} is required.")
            .MaximumLength(Article.TitleMaxLength)
            .WithMessage($"{nameof(Article.Title)} must be {Article.TitleMaxLength} characters or less.")
            .MustAsync(async (title, cancellationToken) => !await articleRepository.ValueForThatFieldExistsAsync(title, nameof(Article.Title), cancellationToken))
            .WithMessage($"{nameof(Article.Title)} already exists.");

        RuleFor(request => request.Content)
            .NotEmpty()
            .WithMessage($"{nameof(Article.Content)} is required")
            .MaximumLength(500)
            .WithMessage($"{nameof(Article.Content)} must be {Article.ContentMaxLength} characters or less.");
    }
}

internal sealed class CreateArticleCommandHandler : ICommandHandler<CreateArticleCommand, BaseResponse<string>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IHashIdService _hashIdService;

    public CreateArticleCommandHandler(IArticleRepository articleRepository, IHashIdService hashIdService)
    {
        _articleRepository = articleRepository;
        _hashIdService = hashIdService;
    }

    public async Task<BaseResponse<string>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = request.Adapt<Article>();

        await _articleRepository.AddAsync(article, cancellationToken);

        string hashedId = _hashIdService.Encode(article.Id);
        return BaseResponse<string>.FromSuccess(hashedId);
    }
}