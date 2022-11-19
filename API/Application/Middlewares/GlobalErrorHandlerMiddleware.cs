using System.Web;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Application.Errors;

namespace Application.Middlewares;

internal class GlobalErrorHandlerMiddleware
{
    public RequestDelegate _next { get; }
    public GlobalErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        HttpStatusCode status = HttpStatusCode.InternalServerError;
        string errorMessage = exception.Message;
        string stackTrace = exception.StackTrace == null ? "" : exception.StackTrace.ToString();
        string searchInGoogle = exception.StackTrace == null ? "" :
            "https://www.google.com/search?q=" + HttpUtility.UrlEncode(errorMessage);

        if (exception is BaseException)
        {
            HttpStatusCode? statusCode = ((BaseException) exception).StatusCode ;
            status = statusCode == null ? HttpStatusCode.BadRequest : (HttpStatusCode) statusCode;
        }

        var result = JsonSerializer.Serialize(new { status, errorMessage, stackTrace, searchInGoogle });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 401;
        return context.Response.WriteAsync(result);
    }
}