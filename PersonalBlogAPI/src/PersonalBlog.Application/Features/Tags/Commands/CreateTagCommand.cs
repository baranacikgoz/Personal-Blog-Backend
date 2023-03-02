using FluentValidation;
using Mapster;
using PersonalBlog.Application.Features.Abstractions;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Application.Interfaces.Repository;
using PersonalBlog.Application.Wrappers;
using PersonalBlog.Domain.Entities;

namespace PersonalBlog.Application.Features.Tags.Commands;

public sealed record CreateTagCommand(string Name) : ICommand<BaseResponse<string>>;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator(ITagRepository tagRepository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(Tag.NameMaxLength)
            .WithMessage("{PropertyName} must be {MaxLength} characters or less.")
            .MustAsync(async (name, cancellationToken) => !await tagRepository.ValueForThatFieldExistsAsync(name, nameof(Tag.Name), cancellationToken))
            .WithMessage("{PropertyName} already exists.");
    }
}

internal sealed class CreateTagCommandHandler : ICommandHandler<CreateTagCommand, BaseResponse<string>>
{
    private readonly ITagRepository _tagRepository;
    private readonly IHashIdService _hashIdService;

    public CreateTagCommandHandler(ITagRepository tagRepository, IHashIdService hashIdService)
    {
        _tagRepository = tagRepository;
        _hashIdService = hashIdService;
    }

    public async Task<BaseResponse<string>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var result = request.Adapt<Tag>();

        await _tagRepository.AddAsync(result, cancellationToken);

        string hashedId = _hashIdService.Encode(result.Id);
        return BaseResponse<string>.FromSuccess(hashedId);
    }
}