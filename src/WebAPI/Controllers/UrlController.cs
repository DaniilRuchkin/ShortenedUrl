using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Application.DTOs;
using URLShortener.Application.Responses;
using URLShortener.Application.Url.Commands.Create;
using URLShortener.Application.Url.Commands.Delete;
using URLShortener.Application.Url.Queries.Get;

namespace URLShortener.Web.Controllers;

[Route("api/v1/url")]
[ApiController]
public class UrlController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateShortUrlAsync(CreateUrlDto createUrlDto)
    {
        var command = new CreateUrlCommand(createUrlDto.Url!, createUrlDto.Password!, Request.Host.Value, Request.Scheme);
        var shortenedPath = await sender.Send(command);

        var response = new BaseResponse<CreateDto>
        {
            Data = new CreateDto
            {
                Url = shortenedPath.Url,
            },
        };

        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUrlAsync(DeleteUrlCommand deleteUrlCommand)
    {
        await sender.Send(deleteUrlCommand);

        return Ok();
    }

    [HttpGet("{shortUrlId}")]
    public async Task<IActionResult> RedirectUrlAsync(string shortUrlId)
    {
        var query = new GetUrlQuery(shortUrlId);
        var url = await sender.Send(query);

        if (url == null)
        {
            return BadRequest();
        }

        return Redirect(url.Url!);
    }
}