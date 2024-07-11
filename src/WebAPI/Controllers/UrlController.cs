using Application.Commands;
using Application.DTOs;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/v1/url")]
[ApiController]
public class UrlController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateShortUrlAsync(CreateUrlCommand command)
    {
        var shortenedPath = await sender.Send(command);
        var shortenedUrl = $"{Request.Scheme}://{Request.Host}/{shortenedPath.ShortenedUrl}";

        var response = new BaseResponse<CreateDataDto>
        {
            Data = new CreateDataDto
            {
                ShortenedUrl = shortenedUrl,
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
}