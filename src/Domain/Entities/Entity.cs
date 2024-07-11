using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Entity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortenedUrl { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}