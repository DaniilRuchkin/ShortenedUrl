using Application.Url.Queries.Get;
using MediatR;

namespace Web.Middlewares;

public class RedirectMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var path = context.Request.Path.ToString().Trim('/');
        var query = new GetUrlQuery(path)
        {
            shortUrl = path,
        };

        if (!string.IsNullOrEmpty(path))
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ISender>();

                var shortUrl = await dbContext.Send(query);

                if (shortUrl != null)
                {
                    context.Response.Redirect(shortUrl.Url!);

                    return;
                }
            }
        }

        await next(context);
    }
}