using FluentValidation;
using HashidsNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PersonalBlog.Application.Exceptions;
using PersonalBlog.Application.Wrappers;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace PersonalBlog.Infrastructure.Middlewares;

public class ExceptionHandlingAndResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly Stopwatch _timer;

    public ExceptionHandlingAndResponseLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
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

            LogResponse(context, elapsedMilliseconds);
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

                default:
                    var displayedErrorMessage =
                        _env.IsProduction() ? "An internal server error has occured."
                        : exception.Message;

                    response = BaseResponse<object>.FromFailure(new List<string> { displayedErrorMessage });
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    break;
            }

            LogErrorWithRequestInfo(context, requestBodyOrQueryString, elapsedMilliseconds);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
        finally
        {
            _timer.Reset();
        }
    }

    private static void LogResponse(HttpContext context, long elapsedMilliseconds)
    {
        if (elapsedMilliseconds > 2000)
        {
            Log.Warning("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms", context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMilliseconds);
            return;
        }

        Log.Information("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms", context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMilliseconds);
    }

    private static void LogErrorWithRequestInfo(HttpContext context, string requestBodyOrQueryString, long elapsedMilliseconds)
    {
        Log.Error("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms for {RequestBodyOrQueryString}", context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMilliseconds, requestBodyOrQueryString);
    }
}