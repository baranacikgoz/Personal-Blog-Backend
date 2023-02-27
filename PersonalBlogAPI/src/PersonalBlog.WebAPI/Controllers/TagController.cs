using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PersonalBlog.Application.Dtos;
using PersonalBlog.Application.Features.Tags.Commands;
using PersonalBlog.Application.Features.Tags.Queries;
using PersonalBlog.Application.Wrappers;
using PersonalBlog.Infrastructure.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace PersonalBlog.WebAPI.Controllers;

[OutputCache(PolicyName = OutputCacheConstants.TagPolicyName)]
public class TagController : BaseController
{
    [HttpGet]
    [SwaggerOperation(OperationId = nameof(GetAll), Summary = "Get all tags.")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<TagDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllTagsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(OperationId = nameof(Create), Summary = "Create tag.")]
    [ProducesResponseType(typeof(BaseResponse<Guid>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateTagCommand command, IOutputCacheStore outputCacheStore, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCacheConstants.TagPolicyKey, cancellationToken);
        return Ok(result);
    }
}