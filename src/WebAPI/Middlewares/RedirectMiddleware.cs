using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Web.Middlewares;

public class RedirectMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var path = context.Request.Path.ToString().Trim('/');

        if (!string.IsNullOrEmpty(path))
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UrlDbContext>();

                var shortUrl = await dbContext.ShortUrl
                    .AsNoTracking()
                    .FirstOrDefaultAsync(url => url.ShortenedUrl == path);

                if (shortUrl != null)
                {
                    context.Response.Redirect(shortUrl.OriginalUrl);

                    return;
                }
            }
        }

        await next(context);
    }
}