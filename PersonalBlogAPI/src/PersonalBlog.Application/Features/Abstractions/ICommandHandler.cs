using MediatR;
using PersonalBlog.Application.Features.Abstractions;

namespace PersonalBlog.Application.Features;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
 where TCommand : ICommand<TResponse>
 where TResponse : notnull
{
}