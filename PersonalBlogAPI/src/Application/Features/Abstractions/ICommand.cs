using MediatR;

namespace Application.Features.Abstractions;

public interface ICommand<out T> : IRequest<T> where T : notnull
{
}