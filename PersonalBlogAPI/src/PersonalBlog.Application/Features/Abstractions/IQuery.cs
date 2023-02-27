using MediatR;

namespace PersonalBlog.Application.Features.Abstractions;

public interface IQuery<out T> : IRequest<T> where T : notnull
{
}