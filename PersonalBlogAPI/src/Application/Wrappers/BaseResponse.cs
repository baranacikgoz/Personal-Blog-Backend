using FluentValidation.Results;

namespace Application.Wrappers;

public sealed class BaseResponse<T>
{
    public bool IsSucceded { get; }
    public ICollection<string>? ErrorMessages { get; }
    public T? Data { get; }

    public static BaseResponse<T> FromSuccess(T data) => new(data);

    public static BaseResponse<T> FromFailure(ICollection<string> errorMessages) => new(errorMessages);

    public static BaseResponse<T> FromFailure(List<ValidationFailure> validationFailures) => new(validationFailures);

    private BaseResponse(T data)
    {
        IsSucceded = true;
        ErrorMessages = null;
        Data = data;
    }

    private BaseResponse(ICollection<string> errorMessages)
    {
        IsSucceded = false;
        ErrorMessages = errorMessages;
    }

    private BaseResponse(List<ValidationFailure> validationFailures)
    {
        IsSucceded = false;
        ErrorMessages = validationFailures.ConvertAll(failures => failures.ErrorMessage);
    }
}