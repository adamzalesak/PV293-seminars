using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var response = exception switch
        {
            ValidationException validationException => HandleValidationException(context, validationException),
            InvalidOperationException invalidOperationException => HandleInvalidOperationException(context, invalidOperationException),
            ArgumentException argumentException => HandleArgumentException(context, argumentException),
            UnauthorizedAccessException unauthorizedAccessException => HandleUnauthorizedAccessException(context, unauthorizedAccessException),
            _ => HandleGenericException(context, exception)
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }

    private static object HandleValidationException(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => char.ToLowerInvariant(g.Key[0]) + g.Key[1..], // camelCase the property names
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Errors = errors,
            Instance = context.Request.Path
        };
    }

    private static object HandleInvalidOperationException(HttpContext context, InvalidOperationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = exception.Message,
            Instance = context.Request.Path
        };
    }

    private static object HandleArgumentException(HttpContext context, ArgumentException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Invalid Argument",
            Status = StatusCodes.Status400BadRequest,
            Detail = exception.Message,
            Instance = context.Request.Path
        };
    }

    private static object HandleUnauthorizedAccessException(HttpContext context, UnauthorizedAccessException exception)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Title = "Forbidden",
            Status = StatusCodes.Status403Forbidden,
            Detail = exception.Message,
            Instance = context.Request.Path
        };
    }

    private static object HandleGenericException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occurred while processing your request.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "An unexpected error occurred. Please try again later.",
            Instance = context.Request.Path
        };
    }
}

public class ValidationProblemDetails : ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}