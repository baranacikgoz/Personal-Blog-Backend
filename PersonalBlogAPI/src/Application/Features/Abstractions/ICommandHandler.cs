using MediatR;
using Application.Features.Abstractions;

namespace Application.Features;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
 where TCommand : ICommand<TResponse>
 where TResponse : notnull
{
}