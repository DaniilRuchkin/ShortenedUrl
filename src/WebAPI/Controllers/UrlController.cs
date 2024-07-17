using Application.DTOs;
using Application.Responses;
using Application.Url.Commands.Create;
using Application.Url.Commands.Delete;
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
        var shortenedUrl = $"{Request.Scheme}://{Request.Host}/{shortenedPath.Url}";

        var response = new BaseResponse<CreateDto>
        {
            Data = new CreateDto
            {
                Url = shortenedUrl,
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