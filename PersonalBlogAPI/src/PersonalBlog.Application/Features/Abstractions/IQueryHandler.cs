using MediatR;
using PersonalBlog.Application.Features.Abstractions;

namespace PersonalBlog.Application.Features;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
}