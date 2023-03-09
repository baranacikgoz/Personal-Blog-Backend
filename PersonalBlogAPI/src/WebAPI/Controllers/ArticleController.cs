using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Application.Dtos;
using Application.Features.Articles.Commands;
using Application.Features.Articles.Queries;
using Application.Wrappers;
using Infrastructure.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace WebAPI.Controllers;

[OutputCache(PolicyName = OutputCacheConstants.ArticlePolicyName)]
public class ArticleController : BaseController
{
    [HttpGet]
    [SwaggerOperation(OperationId = nameof(GetById), Summary = "Get article by id.")]
    [ProducesResponseType(typeof(BaseResponse<ArticleWithTagsDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetArticleByIdIncludeTagsQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(OperationId = nameof(GetAll), Summary = "Get all articles.")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ArticleDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllArticlesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(OperationId = nameof(Create), Summary = "Create article.")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateArticleCommand command, IOutputCacheStore outputCacheStore, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCacheConstants.ArticlePolicyKey, cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    [SwaggerOperation(OperationId = nameof(AddExistingTagToArticle), Summary = "Add tag to the article.")]
    [ProducesResponseType(typeof(BaseResponse<ArticleWithTagsDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddExistingTagToArticle(AddExistingTagToArticleCommand command, IOutputCacheStore outputCacheStore, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCacheConstants.ArticlePolicyKey, cancellationToken);
        return Ok(result);
    }
}