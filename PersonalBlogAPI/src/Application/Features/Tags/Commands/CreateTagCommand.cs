using FluentValidation;
using Mapster;
using Application.Features.Abstractions;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Wrappers;
using Domain.Entities;

namespace Application.Features.Tags.Commands;

public sealed record CreateTagCommand(string Name) : ICommand<BaseResponse<string>>;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator(ITagRepository tagRepository)
    {
        _ = RuleFor(request => request.Name)
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
        Tag tag = request.Adapt<Tag>();

        _ = await _tagRepository.AddAsync(tag, cancellationToken);

        string hashedId = _hashIdService.Encode(tag.Id);
        return BaseResponse<string>.FromSuccess(hashedId);
    }
}