using MongoDB.Bson;

namespace AnalyticsService.Infrastructure.Persistence.Mongo.Entities;

public record MongoBaseEntity
{
    public ObjectId Id { get; set; }
}
