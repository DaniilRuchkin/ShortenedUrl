using URLShortener.Application.Responses;

namespace URLShortener.Web.Middlewares;

public class ErrorMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NullReferenceException ex)
        {
            await ExcentionAsync(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (UnauthorizedAccessException ex)
        {
            await ExcentionAsync(context, ex, StatusCodes.Status401Unauthorized);
        }
        catch (Exception ex)
        {
            await ExcentionAsync(context, ex, StatusCodes.Status500InternalServerError);
        }
    }

    private static Task ExcentionAsync(HttpContext context, Exception ex, int statusCode)
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