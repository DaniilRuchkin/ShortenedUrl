using Application.DTOs;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Application.Responses;
using URLShortener.Application.Url.Commands.Create;
using URLShortener.Application.Url.Commands.Delete;
using URLShortener.Application.Url.Queries.Get;

namespace URLShortener.Web.Controllers;

[ApiController]
public class UrlController(ISender sender) : ControllerBase
{
    [HttpPost("api/v1/url")]
    public async Task<IActionResult> CreateShortUrlAsync(CreateUrlRequest createUrlRequest)
    {
        var command = new CreateUrlCommand(createUrlRequest.Url!, createUrlRequest.Password!, Request.Host.Value, Request.Scheme);
        var shortenedPath = await sender.Send(command);

        var response = new BaseResponse<CreateShortUrlResponse>
        {
            Data = new CreateShortUrlResponse
            {
                Url = shortenedPath.Url,
            },
        };

        return Ok(response);
    }

    [HttpDelete("api/v1/url")]
    public async Task<IActionResult> DeleteUrlAsync(DeleteUrlCommand deleteUrlCommand)
    {
        await sender.Send(deleteUrlCommand);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> RedirectUrlAsync(string id)
    {
        var query = new GetUrlQuery(id);
        var url = await sender.Send(query);

        if (url == null)
        {
            return BadRequest();
        }

        return Redirect(url.Url!);
    }
}