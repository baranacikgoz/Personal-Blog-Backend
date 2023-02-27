using MediatR;

namespace PersonalBlog.Application.Features.Abstractions;

public interface ICommand<out T> : IRequest<T> where T : notnull
{
}