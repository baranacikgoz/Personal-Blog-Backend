using FluentValidation;
using Mapster;
using Application.Features.Abstractions;
using Application.Interfaces;
using Application.Interfaces.Repository.ReadRepositories;
using Application.Wrappers;
using Domain.Entities;

namespace Application.Features.Articles.Commands;

public sealed record CreateArticleCommand(string Title, string Content) : ICommand<BaseResponse<string>>;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator(IArticleRepository articleRepository)
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(Article.TitleMaxLength)
            .WithMessage("{PropertyName} must be at most {MaxLength} characters.")
            .MustAsync(async (title, cancellationToken) => !await articleRepository.ValueForThatFieldExistsAsync(title, nameof(Article.Title), cancellationToken))
            .WithMessage("{PropertyName} already exists.");

        RuleFor(request => request.Content)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .MaximumLength(Article.ContentMaxLength)
            .WithMessage("{PropertyName} must be at most {MaxLength} characters.");
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