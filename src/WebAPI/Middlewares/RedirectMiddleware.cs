using Domain.Repositories;

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
                var urlRepository = scope.ServiceProvider.GetRequiredService<IQueryUrlRepository>();
                var shortUrl = await urlRepository.GetUrlAsync(path, context.RequestAborted);

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