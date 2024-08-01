using URLShortener.Application.Responses;
using FluentValidation;

namespace URLShortener.Web.Middlewares;

public class ErrorMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException ex)
        {
            await WriteValidationResponseAsync(context, ex);
        }
        catch (NullReferenceException ex)
        {
            await WriteExceptionResponseAsync(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteExceptionResponseAsync(context, ex, StatusCodes.Status401Unauthorized);
        }
        catch (Exception ex)
        {
            await WriteExceptionResponseAsync(context, ex, StatusCodes.Status500InternalServerError);
        }
    }
    private static Task WriteValidationResponseAsync(HttpContext context, ValidationException ex)
    {
        var groupedErrors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .Select(g => $"{g.Key}: {string.Join("; ", g.Select(e => e.ErrorMessage).Distinct())}");

        var response = new BaseResponse<object>
        {
            Data = null,
            Error = string.Join("; ", groupedErrors)
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return context.Response.WriteAsJsonAsync(response);
    }
    private static Task WriteExceptionResponseAsync(HttpContext context, Exception ex, int statusCode)
    {
        var response = new BaseResponse<object>()
        {
            Data = null,
            Error = ex.Message
        };

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(response);
    }
}