using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace URLShortener.Domain.Entities;

public class ShortUrl
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string? OriginalUrl { get; set; }

    public string? ShortenedUrl { get; set; }

    public string? Password { get; set; }
}