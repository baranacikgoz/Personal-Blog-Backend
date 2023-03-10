using Mapster;
using Application.Dtos;
using Application.Features.Abstractions;
using Application.Interfaces.Repository;
using Application.Wrappers;

namespace Application.Features.Tags.Queries;

public sealed record GetAllTagsQuery() : IQuery<BaseResponse<IEnumerable<TagDto>>>;

internal sealed class GetAllTagsQueryHandler : IQueryHandler<GetAllTagsQuery, BaseResponse<IEnumerable<TagDto>>>
{
    private readonly ITagRepository _tagRepository;

    public GetAllTagsQueryHandler(ITagRepository tagRepository) => _tagRepository = tagRepository;

    public async Task<BaseResponse<IEnumerable<TagDto>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var allTags = await _tagRepository.GetAllAsync(cancellationToken);

        var dto = allTags.Adapt<IEnumerable<TagDto>>();

        return BaseResponse<IEnumerable<TagDto>>.FromSuccess(dto);
    }
}