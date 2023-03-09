using MediatR;

namespace Application.Features.Abstractions;

public interface IQuery<out T> : IRequest<T> where T : notnull
{
}