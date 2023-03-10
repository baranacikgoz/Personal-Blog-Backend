using FluentValidation;
using HashidsNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Application.Exceptions;
using Application.Wrappers;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Application.Extensions;

namespace Infrastructure.Middlewares;

public class ExceptionHandlingAndResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger _logger;
    private readonly Stopwatch _timer;

    public ExceptionHandlingAndResponseLoggingMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        ILogger logger
        )
    {
        _next = next;
        _env = env;
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task Invoke(HttpContext context)
    {
        _timer.Start();
        long elapsedMilliseconds;

        string requestBodyOrQueryString = string.Empty;

        var request = context.Request;

        if (!string.IsNullOrEmpty(request.ContentType)
            && request.ContentType.StartsWith("application/json"))
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 4096, true);
            requestBodyOrQueryString = await reader.ReadToEndAsync();

            request.Body.Position = 0;
        }
        else if (string.IsNullOrEmpty(request.ContentType))
        {
            requestBodyOrQueryString = request.QueryString.Value ?? String.Empty;
        }

        try
        {
            await _next(context);
            elapsedMilliseconds = _timer.ElapsedMilliseconds;

            _logger.LogHttpSuccess(context, elapsedMilliseconds);
        }
        catch (Exception exception)
        {
            var type = exception.GetType();

            elapsedMilliseconds = _timer.ElapsedMilliseconds;

            BaseResponse<object> response;

            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case EntityNotFoundException ex:
                    response = BaseResponse<object>.FromFailure(new List<string> { ex.Message });
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                    break;

                case ValidationException ex:
                    response = BaseResponse<object>.FromFailure(ex.Errors.ToList());
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    break;

                case NoResultException _: // From HashIdService
                    response = BaseResponse<object>.FromFailure(new List<string> { "Invalid Id." });
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                    break;

                case DbUpdateException _:
                    // Occurs when a unique index is violated

                    // This exception should not be thrown because uniqueness of the values should be checked at validators.
                    response = BaseResponse<object>.FromFailure(
                        new List<string> {
                             @"
                             A record with the same value(s) already exists.
                             "}
                             );
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    break;

                default:
                    var displayedErrorMessage =
                        _env.IsProduction() ? "An internal server error has occured."
                        : exception.Message;

                    response = BaseResponse<object>.FromFailure(new List<string> { displayedErrorMessage });
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    break;
            }

            _logger.LogHttpErrorWithRequestInfo(context, requestBodyOrQueryString, elapsedMilliseconds);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
        finally
        {
            _timer.Reset();
        }
    }
}