using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Application.Dtos;
using Application.Features.Tags.Commands;
using Application.Features.Tags.Queries;
using Application.Wrappers;
using Infrastructure.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace WebAPI.Controllers;

[OutputCache(PolicyName = OutputCacheConstants.TagPolicyName)]
public class TagController : BaseController
{
    [Tags("Tag:Read")]
    [HttpGet]
    [SwaggerOperation(OperationId = nameof(GetAll), Summary = "Get all tags.")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<TagDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        BaseResponse<IEnumerable<TagDto>> result = await Mediator.Send(new GetAllTagsQuery(), cancellationToken);
        return Ok(result);
    }

    [Tags("Tag:Write")]
    [HttpPost]
    [SwaggerOperation(OperationId = nameof(Create), Summary = "Create tag.")]
    [ProducesResponseType(typeof(BaseResponse<Guid>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateTagCommand command, IOutputCacheStore outputCacheStore, CancellationToken cancellationToken)
    {
        BaseResponse<string> result = await Mediator.Send(command, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCacheConstants.TagPolicyKey, cancellationToken);
        return Ok(result);
    }
}