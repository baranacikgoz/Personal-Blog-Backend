using Microsoft.AspNetCore.Http;

using Serilog;
using Serilog.Events;

namespace Application.Extensions;

public static class SerilogExtensions
{
    public static void LogHttpErrorWithRequestInfo(this ILogger logger
    , HttpContext context, string requestBodyOrQueryString, long elapsedMilliseconds)
    {
        logger.Error("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms for {RequestBodyOrQueryString}",
            context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMilliseconds, requestBodyOrQueryString);
    }

    public static void LogHttpSuccess(this ILogger logger, HttpContext context, long elapsedMilliseconds)
    {
        if (elapsedMilliseconds > 2000)
        {
            LogHttpSuccess(logger, context, elapsedMilliseconds, LogEventLevel.Warning);
            return;
        }

        LogHttpSuccess(logger, context, elapsedMilliseconds, LogEventLevel.Information);
    }

    private static void LogHttpSuccess(ILogger logger, HttpContext context, long elapsedMilliseconds, LogEventLevel logLevel)
    {
        logger.Write(logLevel, "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms",
            context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMilliseconds);
    }
}