namespace URLShortener.Application.DTOs;

public class DeleteUrlDto
{
    public string? ShortenedUrl { get; set; }

    public string? Password { get; set; }
}