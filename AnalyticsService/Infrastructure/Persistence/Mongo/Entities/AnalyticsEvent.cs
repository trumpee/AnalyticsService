using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Infrastructure.Persistence.Mongo.Entities;

public record AnalyticsEvent : MongoBaseEntity
{
    [BsonElement("eventId")]
    public required string NotificationId { get; init; }

    [BsonElement("streamId")]
    public required string StreamId { get; init; }

    [BsonElement("action")]
    public required string Action { get; init; }

    [BsonElement("version")]
    public required string Version { get; init; }

    [BsonElement("source")]
    public required string Source { get; init; }

    [BsonElement("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

    [BsonElement("metadata")]
    public Dictionary<string, string>? Metadata { get; init; }

    [BsonElement("payload")]
    public required string Payload { get; init; }
}
