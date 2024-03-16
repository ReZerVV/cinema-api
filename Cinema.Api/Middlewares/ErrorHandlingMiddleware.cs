using Cinema.Application.Utils;
using System.Net;

namespace Cinema.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        object response = exception switch
        {
            CinemaError cinemaError => new { Error = new { Type = cinemaError.Type, Message = cinemaError.Message } },
            _ => new { Error = new { Message = exception.Message } },
        };
        await context.Response.WriteAsJsonAsync(response);
    }
}